using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using System.Diagnostics.Eventing.Reader; // استيراد مكتبة Eventing.Reader لقراءة سجلات الأحداث
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات

namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public class clsTest // كلاس لإدارة الاختبارات في طبقة الأعمال
    {

        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)

        // خصائص الاختبار
        public int TestID { set; get; } // رقم الاختبار
        public int TestAppointmentID { set; get; } // رقم موعد الاختبار
        public clsTestAppointment TestAppointmentInfo { set; get; } // معلومات موعد الاختبار
        public bool TestResult { set; get; } // نتيجة الاختبار (true = نجح، false = فشل)
        public string Notes { set; get; } // ملاحظات حول الاختبار
        public int CreatedByUserID { set; get; } // رقم المستخدم الذي أنشأ السجل
       
        // Constructor الافتراضي - يُستخدم عند إنشاء اختبار جديد
        public clsTest()

        {
            this.TestID = -1; // -1 يعني اختبار جديد لم يُحفظ بعد
            this.TestAppointmentID = -1; // -1 يعني لم يتم ربط موعد بعد
            this.TestResult = false; // تهيئة النتيجة إلى false (فشل)
            this.Notes =""; // تهيئة الملاحظات كسلسلة فارغة
            this.CreatedByUserID = -1; // -1 يعني لم يتم تعيين مستخدم بعد

            Mode = enMode.AddNew; // تعيين الوضع إلى إضافة جديدة

        }

        public clsTest(int TestID,int TestAppointmentID,
            bool TestResult, string Notes, int CreatedByUserID)

        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;
            this.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID,
                this.TestResult,this.Notes,this.CreatedByUserID);
              

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID,
                this.TestResult, this.Notes, this.CreatedByUserID);
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = "";int CreatedByUserID = -1;

            if (clsTestData.GetTestInfoByID( TestID,
            ref  TestAppointmentID, ref  TestResult,
            ref  Notes, ref  CreatedByUserID))

                return new clsTest(TestID,
                        TestAppointmentID,  TestResult,
                        Notes,  CreatedByUserID);
            else
                return null;

        }

        public static clsTest FindLastTestPerPersonAndLicenseClass
            (int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; int CreatedByUserID = -1;

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID,LicenseClassID,(int) TestTypeID, ref TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes, ref CreatedByUserID))

                return new clsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes, CreatedByUserID);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool  PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }

    }
}
