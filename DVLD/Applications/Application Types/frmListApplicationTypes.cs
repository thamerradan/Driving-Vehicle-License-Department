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
    public partial class frmManageApplicationTypes : Form
    {
        private  DataTable _dtAllApplicationTypes;

        // Constructor للنموذج
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        // حدث النقر على زر الإغلاق
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close(); // إغلاق النموذج
        }

        // حدث تحميل النموذج (يتم تنفيذه عند فتح النموذج)
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            // الحصول على جميع أنواع التطبيقات من قاعدة البيانات
            _dtAllApplicationTypes = clsApplicationType.GetAllApplicationTypes();
            // ربط جدول البيانات بالشبكة (DataGridView)
            dgvApplicationTypes.DataSource = _dtAllApplicationTypes;
            // عرض عدد السجلات
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();

            // تخصيص أعمدة الشبكة (العناوين والعرض)
            dgvApplicationTypes.Columns[0].HeaderText = "ID"; // عنوان العمود الأول
            dgvApplicationTypes.Columns[0].Width = 110; // عرض العمود الأول
         
            dgvApplicationTypes.Columns[1].HeaderText = "Title"; // عنوان العمود الثاني
            dgvApplicationTypes.Columns[1].Width = 400; // عرض العمود الثاني
       
            dgvApplicationTypes.Columns[2].HeaderText = "Fees"; // عنوان العمود الثالث
            dgvApplicationTypes.Columns[2].Width = 100; // عرض العمود الثالث

           
        }

        // حدث النقر على قائمة "Edit" (تعديل)
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // إنشاء وعرض نموذج تعديل نوع التطبيق المحدد
            // الحصول على رقم نوع التطبيق من الصف المحدد في الشبكة
            frmEditApplicationType frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog(); // عرض النموذج كحوار
            frmManageApplicationTypes_Load(null, null); // إعادة تحميل قائمة أنواع التطبيقات بعد الإغلاق

        }
    }
}
