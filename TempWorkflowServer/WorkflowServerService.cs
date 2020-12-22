﻿using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.ServiceModel;
using DevExpress.ExpressApp.Workflow.Server;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Workflow;
using DevExpress.Workflow;
using DevExpress.ExpressApp.MiddleTier;
using InDeptTutorialDemo.Module;


namespace TempWorkflowServer.WorkflowServerService {
    public partial class TempWorkflowServerWorkflowServer  : System.ServiceProcess.ServiceBase {
        private WorkflowServer server;
        protected override void OnStart(string[] args) {
            if(server == null) {
                ServerApplication serverApplication = new ServerApplication();
                serverApplication.ApplicationName = "InDeptTutorialDemo";
				serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                // The service can only manage workflows for those business classes that are contained in Modules specified by the serverApplication.Modules collection.
                // So, do not forget to add the required Modules to this collection via the serverApplication.Modules.Add method.
                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new InDeptTutorialDemoModule());
                serverApplication.Modules.Add(new SecurityModule());
                serverApplication.Modules.Add(new WorkflowModule());
                serverApplication.Modules.Add(new InDeptTutorialDemo.Module.InDeptTutorialDemoModule());
                if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                    serverApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                serverApplication.Setup();
                serverApplication.Logon();

                IObjectSpaceProvider objectSpaceProvider = serverApplication.ObjectSpaceProvider;

                WorkflowCreationKnownTypesProvider.AddKnownType(typeof(DevExpress.Xpo.Helpers.IdList));

                server = new WorkflowServer("http://localhost:46232", new BasicHttpBinding(), objectSpaceProvider, objectSpaceProvider);
                server.StartWorkflowListenerService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.StartWorkflowByRequestService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.RefreshWorkflowDefinitionsService.DelayPeriod = TimeSpan.FromHours(1);
                server.HostManager.CloseHostTimeout = System.TimeSpan.FromSeconds(20);

                server.CustomizeHost += delegate (object sender, CustomizeHostEventArgs e) {
                    e.WorkflowInstanceStoreBehavior.WorkflowInstanceStore.RunnableInstancesDetectionPeriod =
                        TimeSpan.FromSeconds(15);
                };

                server.CustomHandleException += delegate(object sender, CustomHandleServiceExceptionEventArgs e) {
                    Tracing.Tracer.LogError(e.Exception);
                    e.Handled = false;
                };
            }
            server.Start();


        }
        protected override void OnStop() {
            server.Stop();
        }
        public TempWorkflowServerWorkflowServer() {
            InitializeComponent();
        }
    }
}
