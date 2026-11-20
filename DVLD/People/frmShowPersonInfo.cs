using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.People
{
    public partial class frmShowPersonInfo : Form
    {
        
        // Constructor للنموذج - يُستخدم عند عرض معلومات شخص بناءً على رقم الشخص
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            // تحميل معلومات الشخص في عنصر التحكم (User Control)
            ctrlPersonCard1.LoadPersonInfo(PersonID);

        }

        // Constructor للنموذج - يُستخدم عند عرض معلومات شخص بناءً على الرقم الوطني
        public frmShowPersonInfo(string NationalNo)
        {
            InitializeComponent();
            // تحميل معلومات الشخص في عنصر التحكم باستخدام الرقم الوطني
            ctrlPersonCard1.LoadPersonInfo(NationalNo);
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج
        }
    }
}
