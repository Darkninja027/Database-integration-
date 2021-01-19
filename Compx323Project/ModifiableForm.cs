using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compx323Project
{
    public partial class ModifiableForm : AddForm
    {
        public bool updating = false;

        public ModifiableForm()
        {
            InitializeComponent();
        }

        public virtual void UpdateForm()
        {
            updating = true;
            ShowForm();
        }
    }
}
