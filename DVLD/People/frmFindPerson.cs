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
    public partial class frmFindPerson : Form
    {

        // تعريف Delegate (ممثل) للحدث الذي يتم إطلاقه عند إرجاع البيانات
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // تعريف حدث باستخدام الـ Delegate
        // يتم إطلاق هذا الحدث عند إرجاع رقم الشخص المحدد
        public event DataBackEventHandler DataBack;


        // Constructor للنموذج
        public frmFindPerson()
        {
            InitializeComponent();
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            // إطلاق الحدث لإرسال البيانات (رقم الشخص) إلى النموذج الذي استدعى هذا النموذج
            // ?. يعني التحقق من أن الحدث ليس null قبل إطلاقه
            DataBack?.Invoke(this, ctrlPersonCardWithFilter1.PersonID);
        }
    }
}
