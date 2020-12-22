using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace InDeptTutorialDemo.Module.BusinessObjects
{
    //Contact class
    [DefaultClassOptions]
    public class Contact : Person
    {
        public Contact(Session session) : base(session) { }

        //Page address property
        private string webPageAddress;
        public string WebPageAddress
        {
            get { return webPageAddress; }
            set { SetPropertyValue(nameof(WebPageAddress), ref webPageAddress, value); }
        }

        //Nick name property
        private string nickName;
        public string NickName
        {
            get { return nickName; }
            set { SetPropertyValue(nameof(NickName), ref nickName, value); }
        }

        //Spouse name property
        private string spouseName;
        public string SpouseName
        {
            get { return spouseName; }
            set { SetPropertyValue(nameof(SpouseName), ref spouseName, value); }
        }

        //Title of courtesy property
        private TitleOfCourtesy titleOfCourtesy;
        public TitleOfCourtesy TitleOfCourtesy
        {
            get { return titleOfCourtesy; }
            set { SetPropertyValue(nameof(TitleOfCourtesy), ref titleOfCourtesy, value); }
        }

        //Anniversary property
        private DateTime anniversary;
        public DateTime Anniversary
        {
            get { return anniversary; }
            set { SetPropertyValue(nameof(Anniversary), ref anniversary, value); }
        }

        //Note property
        private string notes;
        [Size(4096)]
        public string Notes
        {
            get { return notes; }
            set { SetPropertyValue(nameof(Notes), ref notes, value); }
        }

        //Position property
        private Position position;
        public Position Position
        {
            get { return position; }
            set { SetPropertyValue(nameof(Position), ref position, value); }
        }

        //Contact-DemoTask Many-to-Many Relationship
        [Association("Contact-DemoTask")]
        public XPCollection<DemoTask> Tasks
        {
            get
            {
                return GetCollection<DemoTask>(nameof(Tasks));
            }
        }

        //Department-Contacts One-to-Many Relationship
        private Department department;
        [Association("Department-Contacts")]
        public Department Department
        {
            get { return department; }
            set { SetPropertyValue(nameof(Department), ref department, value); }
        }

        //Manager property (Department dependent-reference) 
        private Contact manager;
        [DataSourceProperty("Department.Contacts", DataSourcePropertyIsNullMode.SelectAll)]
        //Look for "Manager" Contacts Criteria
        [DataSourceCriteria("Position.Title = 'Manager' AND Oid != '@This.Oid'")]
        public Contact Manager
        {
            get { return manager; }
            set { SetPropertyValue(nameof(Manager), ref manager, value); }
        }
    }

    //Title Enumerator
    public enum TitleOfCourtesy { Dr, Miss, Mr, Mrs, Ms };

    //Priority Enumerator
    public enum Priority {
        [ImageName("State_Priority_Low")]
        Low = 0,
        [ImageName("State_Priority_Normal")]
        Normal = 1,
        [ImageName("State_Priority_High")]
        High = 2
    };

    //Department class
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty(nameof(Title))]
    public class Department : BaseObject
    {
        public Department(Session session) : base(session) { }
        private string title;
        public string Title
        {
            get { return title; }
            set { SetPropertyValue(nameof(Title), ref title, value); }
        }
        private string office;
        public string Office
        {
            get { return office; }
            set { SetPropertyValue(nameof(Office), ref office, value); }
        }

        [Association("Department-Contacts")]
        public XPCollection<Contact> Contacts
        {
            get
            {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }


    }

    //Position class
    [DefaultClassOptions]
    [System.ComponentModel.DefaultProperty(nameof(Title))]
    public class Position : BaseObject
    {
        public Position(Session session) : base(session) { }
        private string title;
        [RuleRequiredField(DefaultContexts.Save)]
        public string Title
        {
            get { return title; }
            set { SetPropertyValue(nameof(Title), ref title, value); }
        }
    }

    //DemoTask class
    [DefaultClassOptions]
    [ModelDefault("Caption", "Demo-Task")]
    public class DemoTask : Task
    {
        public DemoTask(Session session) : base(session) { }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }

        [Association("Contact-DemoTask")]
        public XPCollection<Contact> Contacts
        {
            get
            {
                return GetCollection<Contact>(nameof(Contacts));
            }
        }

        private Priority priority;
        public Priority Priority
        {
            get { return priority; }
            set
            {
                SetPropertyValue(nameof(Priority), ref priority, value);
            }
        }

        [Action(ToolTip = "Postpone the task to the next day")]
        public void Postpone()
        {
            if (DueDate == DateTime.MinValue)
            {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }
    }

    //Workflow classes
    [DefaultClassOptions]
    public class Issue : BaseObject
    {
        public Issue(Session session) : base(session) { }
        public string Subject
        {
            get { return GetPropertyValue<string>(nameof(Subject)); }
            set { SetPropertyValue<string>(nameof(Subject), value); }
        }
        public bool Active
        {
            get { return GetPropertyValue<bool>(nameof(Active)); }
            set { SetPropertyValue<bool>(nameof(Active), value); }
        }
    }
    [DefaultClassOptions]
    public class ITask : BaseObject
    {
        public ITask(Session session) : base(session) { }
        public string Subject
        {
            get { return GetPropertyValue<string>(nameof(Subject)); }
            set { SetPropertyValue<string>(nameof(Subject), value); }
        }
        public Issue Issue
        {
            get { return GetPropertyValue<Issue>(nameof(Issue)); }
            set { SetPropertyValue<Issue>(nameof(Issue), value); }
        }
    }


}