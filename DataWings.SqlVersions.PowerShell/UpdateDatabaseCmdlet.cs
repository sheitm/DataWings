namespace DataWings.SqlVersions.PowerShell
{
    using DataWings.SqlVersions;
    using System;
    using System.Collections.Generic;
    using System.Management.Automation;
    using System.Runtime.CompilerServices;

    [Cmdlet("Update", "Database")]
    public class UpdateDatabaseCmdlet : Cmdlet
    {
        [CompilerGenerated]
        private string _baseDirectory;
        [CompilerGenerated]
        private string _databaseName;
        [CompilerGenerated]
        private string _databaseServer;

        protected override void BeginProcessing()
        {
            IList<Update> updates = new Updater(this.BaseDirectoy, this.DatabaseServer, this.DatabaseName).GetUpdates();
            base.WriteObject(updates, true);
        }

        [Parameter]
        public string BaseDirectoy
        {
            [CompilerGenerated]
            get
            {
                return this._baseDirectory;
            }
            [CompilerGenerated]
            set
            {
                this._baseDirectory = value;
            }
        }

        [Parameter]
        public string DatabaseName
        {
            [CompilerGenerated]
            get
            {
                return this._databaseName;
            }
            [CompilerGenerated]
            set
            {
                this._databaseName = value;
            }
        }

        [Parameter]
        public string DatabaseServer
        {
            [CompilerGenerated]
            get
            {
                return this._databaseServer;
            }
            [CompilerGenerated]
            set
            {
                this._databaseServer = value;
            }
        }
    }
}

