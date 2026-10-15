using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Management.Deployment;

namespace QuickRun
{
    public class StartMenuApp
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public bool IsStoreApp { get; set; }

        public string AppUserModelId { get; set; }
    }
}
