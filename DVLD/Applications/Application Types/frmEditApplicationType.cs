using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmEditApplicationType : Form
    {

        private int _ApplicationTypeID=-1;
        private clsApplicationType _ApplicationType;

        // Constructor للنموذج - يأخذ رقم نوع التطبيق كمعامل
        public frmEditApplicationType(int ApplicationTypeID )
        {
            InitializeComponent();

            _ApplicationTypeID = ApplicationTypeID; // حفظ رقم نوع التطبيق

        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            // عرض رقم نوع التطبيق
            lblApplicationTypeID.Text=_ApplicationTypeID.ToString();

            // البحث عن نوع التطبيق باستخدام رقمه
            _ApplicationType = clsApplicationType.Find(_ApplicationTypeID);

            if (_ApplicationType!=null) // إذا تم العثور على نوع التطبيق
            {
                // تحميل البيانات في الحقول
                txtTitle.Text = _ApplicationType.Title; // العنوان
                txtFees.Text = _ApplicationType.Fees.ToString(); // الرسوم


            }

        }

        // حدث النقر على زر الحفظ
        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren()) // التحقق من صحة جميع الحقول في النموذج
            {
                // هنا لا نتابع لأن النموذج غير صحيح
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // إنهاء الدالة

            }

            // تحديث بيانات نوع التطبيق
            _ApplicationType.Title= txtTitle.Text.Trim(); // العنوان
            _ApplicationType.Fees = Convert.ToSingle( txtFees.Text.Trim()); // الرسوم


            // محاولة حفظ التغييرات
            if (_ApplicationType.Save())
            {
                // إذا نجح الحفظ، عرض رسالة نجاح
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // إذا فشل الحفظ
                // عرض رسالة خطأ
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        // حدث التحقق من صحة حقل العنوان
        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
           
          
            // التحقق من أن الحقل ليس فارغاً
            if (string.IsNullOrEmpty(txtTitle.Text.Trim()))
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtTitle, "Title cannot be empty!"); // عرض رسالة خطأ
            }
            else
            {
                errorProvider1.SetError(txtTitle, null); // إزالة رسالة الخطأ
            };


        }

        // حدث التحقق من صحة حقل الرسوم
        private void txtFees_Validating(object sender, CancelEventArgs e)
        {


            
            // التحقق من أن الحقل ليس فارغاً
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtFees, "Fees cannot be empty!"); // عرض رسالة خطأ
                return; // إنهاء الدالة
            }
            else
            {
                errorProvider1.SetError(txtFees, null); // إزالة رسالة الخطأ

            };

            // التحقق من أن القيمة المدخلة رقم صحيح
            if (!clsValidatoin.IsNumber(txtFees.Text))
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtFees, "Invalid Number."); // عرض رسالة خطأ
            }
            else
            {
                errorProvider1.SetError(txtFees, null); // إزالة رسالة الخطأ
            };

        }

    }
}
