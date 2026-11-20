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

namespace DVLD.User
{
    public partial class frmChangePassword : Form
    {
        private int _UserID; // متغير خاص لتخزين رقم المستخدم
        private clsUser _User; // كائن المستخدم من طبقة الأعمال

        // Constructor للنموذج - يأخذ رقم المستخدم كمعامل
        public frmChangePassword(int UserID )
        {
            InitializeComponent();

            _UserID=UserID; // حفظ رقم المستخدم
        }

        // دالة خاصة لإعادة تعيين القيم الافتراضية للحقول
        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = ""; // مسح حقل كلمة المرور الحالية
            txtNewPassword.Text = ""; // مسح حقل كلمة المرور الجديدة
            txtConfirmPassword.Text = ""; // مسح حقل تأكيد كلمة المرور
            txtCurrentPassword.Focus(); // نقل التركيز إلى حقل كلمة المرور الحالية
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmChangePassword_Load(object sender, EventArgs e)
        {
             _ResetDefualtValues(); // إعادة تعيين القيم الافتراضية

              // البحث عن المستخدم باستخدام رقم المستخدم
              _User = clsUser.FindByUserID(_UserID);

            if (_User == null) // إذا لم يتم العثور على المستخدم
            {
                // هنا لا نتابع لأن النموذج غير صحيح
                MessageBox.Show("Could not Find User with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 this.Close(); // إغلاق النموذج

                return; // إنهاء الدالة

            }
            // تحميل معلومات المستخدم في عنصر التحكم
            ctrlUserCard1.LoadUserInfo(_UserID);

        }

        // حدث التحقق من صحة حقل كلمة المرور الحالية
        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {

            // التحقق من أن الحقل ليس فارغاً
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtCurrentPassword, "Username cannot be blank"); // عرض رسالة خطأ
                return; // إنهاء الدالة
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null); // إزالة رسالة الخطأ
            };

            // التحقق من أن كلمة المرور الحالية تطابق كلمة المرور المحفوظة
            if (_User.Password != txtCurrentPassword.Text.Trim())
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!"); // عرض رسالة خطأ
                return; // إنهاء الدالة
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null); // إزالة رسالة الخطأ
            };
        }

        // حدث التحقق من صحة حقل كلمة المرور الجديدة
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            // التحقق من أن الحقل ليس فارغاً
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtNewPassword, "New Password cannot be blank"); // عرض رسالة خطأ
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null); // إزالة رسالة الخطأ
            };
        }

        // حدث التحقق من صحة حقل تأكيد كلمة المرور
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            // التحقق من أن تأكيد كلمة المرور يطابق كلمة المرور الجديدة
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true; // إلغاء الحدث (منع المتابعة)
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!"); // عرض رسالة خطأ
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null); // إزالة رسالة الخطأ
            };
        }

        // حدث النقر على زر الحفظ
        private void btnSave_Click(object sender, EventArgs e)
        {


            
            if (!this.ValidateChildren()) // التحقق من صحة جميع الحقول في النموذج
            {
                // هنا لا نتابع لأن النموذج غير صحيح
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // إنهاء الدالة
            }

            // تعيين كلمة المرور الجديدة للمستخدم
            _User.Password = txtNewPassword.Text;

            // محاولة حفظ التغييرات
            if (_User.Save())
            {
                // إذا نجح الحفظ، عرض رسالة نجاح
                MessageBox.Show("Password Changed Successfully.",
                   "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information );
                _ResetDefualtValues(); // إعادة تعيين القيم الافتراضية
            }
            else // إذا فشل الحفظ
            {
                // عرض رسالة خطأ
                MessageBox.Show("An Erro Occured, Password did not change.",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج

        }
    }
}
