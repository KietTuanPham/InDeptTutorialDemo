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
    [DefaultClassOptions]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Payment : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Payment(Session session)
            : base(session)
        {
        }

        private double rate;
        public double Rate
        {
            get
            {
                return rate;
            }
            set
            {
                if (SetPropertyValue(nameof(Rate), ref rate, value))
                    OnChanged(nameof(Amount));
            }
        }
        private double hours;
        public double Hours
        {
            get
            {
                return hours;
            }
            set
            {
                if (SetPropertyValue(nameof(Hours), ref hours, value))
                    OnChanged(nameof(Amount));
            }
        }
        [PersistentAlias("Rate * Hours")]
        public double Amount
        {
            get
            {
                object tempObject = EvaluateAlias(nameof(Amount));
                if (tempObject != null)
                {
                    return (double)tempObject;
                }
                else
                {
                    return 0;
                }
            }
        }
    }
}