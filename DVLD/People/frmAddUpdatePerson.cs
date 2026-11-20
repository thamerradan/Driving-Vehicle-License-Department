using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.ComponentModel; // استيراد مكتبة ComponentModel للخصائص والسمات
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Contracts; // استيراد مكتبة Contracts للعقود البرمجية
using System.Drawing; // استيراد مكتبة Drawing للتعامل مع الرسومات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Text.RegularExpressions; // استيراد مكتبة RegularExpressions للتعامل مع التعبيرات النمطية
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية
using DVLD.Classes; // استيراد مساحة الاسم الخاصة بالكلاسات العامة
using DVLD.Properties; // استيراد مساحة الاسم الخاصة بالموارد (Resources)
using DVLD_Buisness; // استيراد طبقة الأعمال (Business Layer)
using System.IO; // استيراد مكتبة IO للتعامل مع الملفات
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button; // استيراد static من VisualStyleElement.Button
using System.Runtime.ConstrainedExecution; // استيراد مكتبة ConstrainedExecution للتنفيذ المقيد

namespace DVLD.People // مساحة الاسم الخاصة بنماذج الأشخاص
{
    public partial class frmAddUpdatePerson : Form // نموذج إضافة/تحديث شخص (partial class لأن Designer.cs يحتوي على باقي الكود)
    {

        // تعريف Delegate (ممثل) للحدث الذي يتم إطلاقه عند إرجاع البيانات
        public delegate void DataBackEventHandler(object sender, int PersonID);

        // تعريف حدث باستخدام الـ Delegate
        // يتم إطلاق هذا الحدث عند إرجاع رقم الشخص بعد الحفظ
        public event DataBackEventHandler DataBack;

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        // تعداد (Enum) لتحديد الجنس
        public enum enGendor { Male = 0, Female = 1 };

        private enMode _Mode; // متغير لتحديد الوضع الحالي (إضافة أو تحديث)
        private int _PersonID = -1; // رقم الشخص (-1 يعني شخص جديد)
        clsPerson _Person; // كائن الشخص من طبقة الأعمال

        // Constructor للنموذج - يُستخدم عند إضافة شخص جديد
        public frmAddUpdatePerson()
        {
            InitializeComponent(); // تهيئة مكونات النموذج
            _Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة

        }

        // Constructor للنموذج - يُستخدم عند تحديث شخص موجود
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent(); // تهيئة مكونات النموذج

            _Mode = enMode.Update; // تعيين الوضع إلى تحديث
            _PersonID = PersonID; // حفظ رقم الشخص المراد تحديثه
        }

        // دالة خاصة لإعادة تعيين القيم الافتراضية للنموذج
        private void _ResetDefualtValues()
        {
            // هذه الدالة ستهيئ وتعيد تعيين القيم الافتراضية
            _FillCountriesInComoboBox(); // ملء قائمة الدول

            if (_Mode == enMode.AddNew) // إذا كان الوضع إضافة جديدة
            {
                lblTitle.Text = "Add New Person"; // تعيين عنوان النموذج
                _Person = new clsPerson(); // إنشاء كائن شخص جديد
            } 
            else // إذا كان الوضع تحديث
            {
                lblTitle.Text = "Update Person"; // تعيين عنوان النموذج
            }

            // تعيين الصورة الافتراضية للشخص حسب الجنس
            if (rbMale.Checked) // إذا كان الجنس ذكر
                pbPersonImage.Image = Resources.Male_512; // استخدام صورة الذكر الافتراضية
            else // إذا كان الجنس أنثى
                pbPersonImage.Image = Resources.Female_512; // استخدام صورة الأنثى الافتراضية

            // إخفاء/إظهار رابط إزالة الصورة في حالة عدم وجود صورة للشخص
            llRemoveImage.Visible = (pbPersonImage.ImageLocation != null);

            // نحدد الحد الأقصى للتاريخ إلى 18 سنة من اليوم، ونحدد القيمة الافتراضية نفسها
            // (لضمان أن الشخص بالغ - 18 سنة على الأقل)
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;

            // يجب ألا نسمح بإضافة عمر أكثر من 100 سنة
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            // هذا سيعين الدولة الافتراضية إلى الأردن
            cbCountry.SelectedIndex = cbCountry.FindString("Jordan");

            // تهيئة الحقول النصية كسلسلة فارغة
            txtFirstName.Text = ""; // الاسم الأول
            txtSecondName.Text = ""; // الاسم الثاني
            txtThirdName.Text = ""; // الاسم الثالث
            txtLastName.Text = ""; // اسم العائلة
            txtNationalNo.Text = ""; // الرقم الوطني
            rbMale.Checked = true; // تعيين الجنس إلى ذكر افتراضياً
            txtPhone.Text = ""; // رقم الهاتف
            txtEmail.Text = ""; // البريد الإلكتروني
            txtAddress.Text = ""; // العنوان


        }

        // دالة خاصة لملء قائمة الدول في ComboBox
        private void _FillCountriesInComoboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries(); // الحصول على جميع الدول من طبقة الأعمال

            // إضافة كل دولة إلى ComboBox
            foreach (DataRow row in dtCountries.Rows)
            {
                cbCountry.Items.Add(row["CountryName"]); // إضافة اسم الدولة
            }
        }

        // دالة خاصة لتحميل بيانات الشخص من قاعدة البيانات
        private void _LoadData()
        {
           
            _Person = clsPerson.Find(_PersonID); // البحث عن الشخص باستخدام رقم الشخص

            if (_Person == null) // إذا لم يتم العثور على الشخص
            {
                MessageBox.Show("No Person with ID = " + _PersonID ,"Person Not Found",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                this.Close(); // إغلاق النموذج
                return; // إنهاء الدالة
            }

            // الكود التالي لن يتم تنفيذه إذا لم يتم العثور على الشخص
            lblPersonID.Text = _PersonID.ToString(); // عرض رقم الشخص
            txtFirstName.Text = _Person.FirstName; // الاسم الأول
            txtSecondName.Text = _Person.SecondName; // الاسم الثاني
            txtThirdName.Text = _Person.ThirdName; // الاسم الثالث
            txtLastName.Text = _Person.LastName; // اسم العائلة
            txtNationalNo.Text = _Person.NationalNo; // الرقم الوطني
            dtpDateOfBirth.Value = _Person.DateOfBirth; // تاريخ الميلاد
            
            if (_Person.Gendor == 0) // إذا كان الجنس ذكر (0)
                rbMale.Checked= true; // تفعيل زر الذكر
            else // إذا كان الجنس أنثى (1)
                rbFemale.Checked = true; // تفعيل زر الأنثى

            txtAddress.Text = _Person.Address; // العنوان
            txtPhone.Text = _Person.Phone; // رقم الهاتف
            txtEmail.Text = _Person.Email; // البريد الإلكتروني
            cbCountry.SelectedIndex = cbCountry.FindString(_Person.CountryInfo.CountryName); // اختيار الدولة

          
            // تحميل صورة الشخص في حالة تم تعيينها
            if (_Person.ImagePath != "")
            {
                pbPersonImage.ImageLocation = _Person.ImagePath; // تعيين مسار الصورة

            }

            // إخفاء/إظهار رابط إزالة الصورة في حالة عدم وجود صورة للشخص
            llRemoveImage.Visible = (_Person.ImagePath != ""); 

        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if(_Mode==enMode.Update)
                _LoadData();
        }

        private bool _HandlePersonImage()
        {

            //this procedure will handle the person image,
            //it will take care of deleting the old image from the folder
            //in case the image changed. and it will rename the new image with guid and 
            // place it in the images folder.

          
                //_Person.ImagePath contains the old Image, we check if it changed then we copy the new image
                if (_Person.ImagePath != pbPersonImage.ImageLocation)
                {
                    if (_Person.ImagePath != "")
                    {
                    //first we delete the old image from the folder in case there is any.

                        try
                        {
                            File.Delete(_Person.ImagePath);
                        }
                        catch (IOException)
                        {
                            // We could not delete the file.
                            //log it later   
                        }
                }

                    if (pbPersonImage.ImageLocation != null)
                    {
                        //then we copy the new image to the image folder after we rename it
                        string SourceImageFile=pbPersonImage.ImageLocation.ToString();

                        if (clsUtil.CopyImageToProjectImagesFolder(ref SourceImageFile))
                        {
                            pbPersonImage.ImageLocation = SourceImageFile;
                             return true;
                        }
                        else
                        {
                            MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro","Validation Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;

            }

           if (! _HandlePersonImage())
                return;

            int NationalityCountryID = clsCountry.Find(cbCountry.Text).ID;

            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNo = txtNationalNo.Text.Trim() ;
            _Person.Email = txtEmail.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.DateOfBirth = dtpDateOfBirth.Value;

            if (rbMale.Checked)
                _Person.Gendor = (short) enGendor.Male;
            else
                _Person.Gendor = (short) enGendor.Female;

            _Person.NationalityCountryID = NationalityCountryID;
            
            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = pbPersonImage.ImageLocation;
            else
                _Person.ImagePath = "";

            if (_Person.Save())
            {
                 lblPersonID.Text = _Person.PersonID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update Person";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

    
                // Trigger the event to send data back to the caller form.
                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            
            
        }

        private void llSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                llRemoveImage.Visible = true;
                // ...
            }
        }

        private void llRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
            pbPersonImage.ImageLocation = null;
          


            if (rbMale.Checked)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            llRemoveImage.Visible = false;
        }

        private void rbFemale_Click(object sender, EventArgs e)
        {
           //change the defualt image to female incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Female_512;
        }

        private void rbMale_Click(object sender, EventArgs e)
        {
            //change the defualt image to male incase there is no image set.
            if (pbPersonImage.ImageLocation == null)
                pbPersonImage.Image = Resources.Male_512;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {

            // First: set AutoValidate property of your Form to EnableAllowFocusChange in designer 
            TextBox Temp = ((TextBox) sender);
            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(Temp, null);
            }

        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            //no need to validate the email incase it's empty.
            if (txtEmail.Text.Trim() == "")
                return;

            //validate email format
            if (!clsValidatoin.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format!");
            }
           else
            { 
                errorProvider1.SetError(txtEmail, null); 
            };

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This field is required!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            //Make sure the national number is not used by another person
            if (txtNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.isPersonExist(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }
        }

    }

    }
