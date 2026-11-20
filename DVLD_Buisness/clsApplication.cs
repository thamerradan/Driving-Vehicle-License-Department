using System; // استيراد مكتبة System الأساسية
using System.Data; // استيراد مكتبة Data للعمل مع البيانات
using DVLD_DataAccess; // استيراد طبقة الوصول للبيانات


namespace DVLD_Buisness // مساحة الاسم الخاصة بطبقة الأعمال (Business Layer)
{
    public   class clsApplication // كلاس لإدارة الطلبات في طبقة الأعمال
    {
        // تعداد (Enum) لتحديد وضع العملية (إضافة جديدة أو تحديث)
        public enum enMode { AddNew = 0, Update = 1 };
        // تعداد (Enum) لتحديد أنواع الطلبات المختلفة
        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense=3,
            ReplaceDamagedDrivingLicense=4, ReleaseDetainedDrivingLicsense=5, NewInternationalLicense=6,RetakeTest=7
        };

        public enMode Mode = enMode.AddNew; // متغير لتحديد الوضع الحالي (افتراضياً إضافة جديدة)
        // تعداد (Enum) لتحديد حالات الطلب
        public enum enApplicationStatus { New=1, Cancelled=2,Completed=3};

        public int ApplicationID { set; get; }
        public int ApplicantPersonID { set; get; }
        public string ApplicantFullName
        {
            get
            {
                return clsPerson.Find(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { set; get; }
        public int ApplicationTypeID { set; get; }
        public clsApplicationType ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; } 
        public string StatusText   
        {
            get { 
            
                switch (ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";  
                }
            }   
           
        }
        public DateTime LastStatusDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByUserID { set; get; }
        public clsUser CreatedByUserInfo;

        public clsApplication()

        {
            this.ApplicationID = -1;
            this.ApplicantPersonID = -1;
            this.ApplicationDate = DateTime.Now;
            this.ApplicationTypeID = -1;
            this.ApplicationStatus = enApplicationStatus.New;
            this.LastStatusDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
           
            Mode = enMode.AddNew;

        }

        private clsApplication(int ApplicationID, int ApplicantPersonID, 
            DateTime ApplicationDate, int ApplicationTypeID,
             enApplicationStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID)

        {
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            Mode = enMode.Update;
        }

        private bool _AddNewApplication()
        {
            //call DataAccess Layer 

            this.ApplicationID = clsApplicationData.AddNewApplication(
                this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte) this.ApplicationStatus,
                this.LastStatusDate, this.PaidFees, this.CreatedByUserID);

            return (this.ApplicationID != -1);
        }

        private bool _UpdateApplication()
        {
            //call DataAccess Layer 

            return clsApplicationData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,
                this.ApplicationTypeID, (byte) this.ApplicationStatus,
                this.LastStatusDate, this.PaidFees, this.CreatedByUserID);
           
        }

        public  static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID=-1;
            DateTime ApplicationDate=DateTime.Now ;  int ApplicationTypeID=-1;
            byte ApplicationStatus =1; DateTime LastStatusDate= DateTime.Now;
            float PaidFees = 0  ;  int CreatedByUserID = -1;

            bool IsFound = clsApplicationData.GetApplicationInfoByID 
                                (
                                    ApplicationID, ref  ApplicantPersonID, 
                                    ref  ApplicationDate, ref  ApplicationTypeID,
                                    ref   ApplicationStatus, ref  LastStatusDate,
                                    ref  PaidFees, ref  CreatedByUserID
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsApplication(ApplicationID,  ApplicantPersonID,
                                     ApplicationDate,  ApplicationTypeID,
                                    (enApplicationStatus) ApplicationStatus,  LastStatusDate,
                                     PaidFees,  CreatedByUserID);
            else
                return null;
        }

        // دالة لإلغاء الطلب (تغيير الحالة إلى Cancelled)
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        // ApplicationStatus = 2 يعني Cancelled (ملغى)
        public bool Cancel()

        {
            return clsApplicationData.UpdateStatus (ApplicationID,2); // استدعاء طبقة الوصول للبيانات
        }

        // دالة لإكمال الطلب (تغيير الحالة إلى Completed)
        // ترجع true إذا تم التحديث بنجاح، و false إذا فشلت العملية
        // ApplicationStatus = 3 يعني Completed (مكتمل)
        public bool SetComplete()

        {
            return clsApplicationData.UpdateStatus(ApplicationID, 3); // استدعاء طبقة الوصول للبيانات
        }

        // دالة لحفظ الطلب (إضافة جديد أو تحديث موجود)
        // ترجع true إذا نجحت العملية، و false إذا فشلت
        public bool Save()
        {
            switch (Mode) // التحقق من الوضع الحالي
            {
                case enMode.AddNew: // إذا كان الوضع إضافة جديدة
                    if (_AddNewApplication()) // محاولة إضافة الطلب
                    {

                        Mode = enMode.Update; // تغيير الوضع إلى تحديث بعد الإضافة الناجحة
                        return true; // إرجاع true للنجاح
                    }
                    else
                    {
                        return false; // إرجاع false للفشل
                    }

                case enMode.Update: // إذا كان الوضع تحديث

                    return _UpdateApplication(); // تحديث الطلب وإرجاع النتيجة

            }

            return false; // إرجاع false في حالة أخرى
        }

        // دالة لحذف الطلب من قاعدة البيانات
        // ترجع true إذا تم الحذف بنجاح، و false إذا فشلت العملية
        public  bool Delete()
        {
            return clsApplicationData.DeleteApplication(this.ApplicationID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود طلب بناءً على رقم الطلب (ApplicationID)
        // ترجع true إذا كان الطلب موجوداً، و false إذا لم يكن موجوداً
        public static bool IsApplicationExist(int ApplicationID)
        {
           return clsApplicationData.IsApplicationExist(ApplicationID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة ثابتة للتحقق من وجود طلب نشط لشخص معين ونوع طلب معين
        // ترجع true إذا كان هناك طلب نشط، و false إذا لم يكن هناك طلب نشط
        public static bool DoesPersonHaveActiveApplication(int PersonID,int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID,ApplicationTypeID); // استدعاء طبقة الوصول للبيانات
        }

        // دالة للتحقق من وجود طلب نشط للشخص الحالي ونوع طلب معين
        // ترجع true إذا كان هناك طلب نشط، و false إذا لم يكن هناك طلب نشط
        public  bool DoesPersonHaveActiveApplication( int ApplicationTypeID)
        {
            return DoesPersonHaveActiveApplication(this.ApplicantPersonID, ApplicationTypeID); // استخدام رقم الشخص الحالي
        }

        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType  ApplicationTypeID)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID,(int) ApplicationTypeID);
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationTypeID,int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID,LicenseClassID );
        }
       
        public  int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this.ApplicantPersonID, ApplicationTypeID);
        }

    }
}
