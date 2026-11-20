using System; // استيراد مكتبة System الأساسية
using System.Collections.Generic; // استيراد مكتبة Collections.Generic للقوائم والمجموعات
using System.IO; // استيراد مكتبة IO للتعامل مع الملفات
using System.Linq; // استيراد مكتبة Linq للاستعلامات
using System.Text; // استيراد مكتبة Text للتعامل مع النصوص
using System.Threading.Tasks; // استيراد مكتبة Tasks للعمليات غير المتزامنة
using System.Windows.Forms; // استيراد مكتبة Windows.Forms للواجهات الرسومية
using DVLD_Buisness; // استيراد طبقة الأعمال (Business Layer)


namespace DVLD.Classes // مساحة الاسم الخاصة بالكلاسات العامة
{
    internal static  class clsGlobal // كلاس ثابت يحتوي على متغيرات ووظائف عامة للتطبيق
    {
        public static clsUser CurrentUser; // متغير عام لتخزين المستخدم الحالي المسجل دخوله

        // دالة لحفظ اسم المستخدم وكلمة المرور في ملف نصي
        // ترجع true إذا نجحت العملية، و false إذا فشلت
        public static bool RememberUsernameAndPassword(string Username, string Password)
        {

            try // محاولة تنفيذ الكود
            {
                // هذا سيحصل على مجلد المشروع الحالي
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();


                // تحديد مسار الملف النصي الذي نريد حفظ البيانات فيه
                string filePath = currentDirectory + "\\data.txt";

                // في حالة أن اسم المستخدم فارغ، احذف الملف
                if (Username=="" && File.Exists(filePath)) 
                { 
                     File.Delete(filePath); // حذف الملف
                    return true; // إرجاع true للنجاح

                }

                // ربط اسم المستخدم وكلمة المرور مع فاصل
                string dataToSave = Username + "#//#"+Password ;

                // إنشاء StreamWriter للكتابة في الملف
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // كتابة البيانات في الملف
                    writer.WriteLine(dataToSave);
                   
                  return true; // إرجاع true للنجاح
                }
            }
            catch (Exception ex) // معالجة الأخطاء
            {
               MessageBox.Show ($"An error occurred: {ex.Message}"); // عرض رسالة الخطأ
                return false; // إرجاع false للفشل
            }

        }

        // دالة للحصول على اسم المستخدم وكلمة المرور المحفوظة
        // ترجع true إذا تم العثور على البيانات، و false إذا لم يتم العثور عليها
        public static bool GetStoredCredential(ref string Username, ref string Password)
        {
            // هذه الدالة ستحصل على اسم المستخدم وكلمة المرور المحفوظة وترجع true إذا تم العثور عليهما و false إذا لم يتم العثور عليهما
            try // محاولة تنفيذ الكود
            {
                // الحصول على مجلد المشروع الحالي
                string currentDirectory = System.IO.Directory.GetCurrentDirectory();

                // مسار الملف الذي يحتوي على بيانات الدخول
                string filePath  = currentDirectory + "\\data.txt";

                // التحقق من وجود الملف قبل محاولة قراءته
                if (File.Exists(filePath))
                {
                    // إنشاء StreamReader للقراءة من الملف
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        // قراءة البيانات سطراً بسطر حتى نهاية الملف
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            Console.WriteLine(line); // طباعة كل سطر في Console (للتطوير)
                            // تقسيم السطر باستخدام الفاصل "#//#" للحصول على اسم المستخدم وكلمة المرور
                            string[] result = line.Split(new string[] { "#//#" }, StringSplitOptions.None);

                            Username = result[0]; // اسم المستخدم هو العنصر الأول
                            Password = result[1]; // كلمة المرور هي العنصر الثاني
                        }
                        return true; // إرجاع true للنجاح
                    }
                }
                else // إذا لم يكن الملف موجوداً
                {
                    return false; // إرجاع false (لا توجد بيانات محفوظة)
                }
            }
            catch (Exception ex) // معالجة الأخطاء
            {
                MessageBox.Show ($"An error occurred: {ex.Message}"); // عرض رسالة الخطأ
                return false; // إرجاع false للفشل
            }

        }
    }
}
