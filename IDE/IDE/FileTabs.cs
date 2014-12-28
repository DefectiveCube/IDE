using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace IDE
{
    public partial class FileTabs : UserControl
    {
        private Dictionary<DocumentId, Document> documents;

        public FileTabs()
        {
            InitializeComponent();
        }

        private void FileTabs_ControlAdded(object sender, ControlEventArgs e)
        {
            Debug.WriteLine("test");
        }
    }
}
