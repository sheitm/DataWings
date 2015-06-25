namespace DataWings.SqlVersions.PowerShell
{
    using DataWings.SqlVersions;
    using System;
    using System.Management.Automation;
    using System.Runtime.CompilerServices;

    [Cmdlet("Create", "Database")]
    public class CreateDatabaseCmdlet : Cmdlet
    {
        [CompilerGenerated]
        private string _databaseName;
        [CompilerGenerated]
        private string _workArea;

        protected override void BeginProcessing()
        {
            string str;
            try
            {
                Creator creator;
                if (this.WorkArea == null)
                {
                    creator = new Creator(this.DatabaseName);
                }
                else
                {
                    creator = new Creator(this.DatabaseName, this.WorkArea);
                }
                str = creator.CreateDatabaseScript();
            }
            catch (Exception exception)
            {
                this.ReportException(exception);
                throw;
            }
            base.WriteObject(str);
        }

        private void ReportException(Exception e)
        {
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
        public string WorkArea
        {
            [CompilerGenerated]
            get
            {
                return this._workArea;
            }
            [CompilerGenerated]
            set
            {
                this._workArea = value;
            }
        }
    }
}

