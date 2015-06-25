namespace DataWings.SqlVersions.PowerShell
{
    using System;
    using System.ComponentModel;
    using System.Management.Automation;

    [RunInstaller(true)]
    public class DataWingsSqlVersionsSnapIn : PSSnapIn
    {
        public override string Description
        {
            get
            {
                return "PowerShell interface against DataWings Sql Versions functionality";
            }
        }

        public override string Name
        {
            get
            {
                return "DataWings_SqlVersions_Snap_In";
            }
        }

        public override string Vendor
        {
            get
            {
                return "DataWings Open Source Project";
            }
        }
    }
}

