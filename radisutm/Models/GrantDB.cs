using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using radisutm.ViewModel;
using radisutm.Models;
using System.Data;
using radisutm.ClassFunction;

namespace radisutm.Models
{
    public class GrantDB
    {
        ClsFunction CF = new ClsFunction();
        public DashboardDataTable GetGrantPI()
        {
           
            var user = LoginUser.GetLogin();

            DashboardDataTable modelDT = new DashboardDataTable();
            DataTable dtblActive = new DataTable();

            string GRANT_PI = "SELECT A.REFERENCE_NO, A.PRO_TITLE, NVL(D.COST_CENTER_CODE,'-'), NVL(G.DESCRIPTION,'-'), " +
            " CASE " +
            "  WHEN H1.PTJ_RA IS NOT NULL " +
            "    THEN H.DESCRIPTION " +
            "  WHEN H1.PTJ_RA IS NULL " +
            "    THEN (SELECT DESKRIPSI FROM HR_JABATAN  WHERE KOD_JABATAN = SUBSTR(B.KOD_PTJ, 0, 5)) " +
            " END, " +
            " (SELECT F.KOD FROM RMC_PTYPEDETAIL F WHERE F.PTYPEDETAIL_PK = A.PTYPEDETAIL_FK) AS GRANT_TYPE, " +
            " DECODE (F.PTYPEDETAIL_CATEGORY,1,'INTERNAL',2,'EXTERNAL') AS GRANT_CATEGORY, " +
            " (SELECT D.NAMA FROM HR_MAKLUMAT_PERIBADI D WHERE D.TKH_HAPUS IS NULL AND D.MAKLUMAT_PERIBADI_PK = A.MAKLUMAT_PERIBADI_FK) AS NAMA, " +
            " (SELECT C.NO_PEKERJA FROM HR_STAF C WHERE C.TKH_HAPUS IS NULL AND C.MAKLUMAT_PERIBADI_FK = A.MAKLUMAT_PERIBADI_FK) AS NO_PEKERJA, " +
            " DECODE ((select NVL(AKUAN_STATUS_FK,0) FROM RMC_PRO WHERE TKH_HAPUS IS NULL AND PRO_ACTIVATED IS NULL AND PSTATUS_ID='123' " +
            "   AND REFERENCE_NO=A.REFERENCE_NO),null,'APPROVED',0,'APPROVED - WAITING COST CENTER',1,'APPROVED - WAITING GENERATE LETTER',204," +
            "   'APPROVED - WAITING FOR ACTIVATION', 205,'APPROVED - WAITING PL DECLARATION', 206,'APPROVED - WAITING PL DECLARATION', 321, 'APPROVED'," +
            "   314,'APPROVED - WAITING FOR ACTIVATION'), " +
            " TO_CHAR(A.TKH_CIPTA,'DD-MM-YYYY') AS TKH_PRO, " +
            " A.PRO_RM, A.SUBMISSION_DATE, A.MAKLUMAT_PERIBADI_FK, A.PSTATUS_ID, G.DESCRIPTION, H.DESCRIPTION, " +
            " A.PTYPEDETAIL_FK, NVL(I.KPI_RU,0), A.AKUAN_STATUS_FK, " +
            " (SELECT MIN (H.END_DATE_PREV) FROM RMC_PM_PINDATEMPOH H WHERE H.TKH_HAPUS IS NULL AND H.REFERENCE_NO = A.REFERENCE_NO) AS DATE_END_PREV, " +
            " H1.PTJ_RA " +
            " ,A.PRO_ACTIVATED " +
            " ,NVL((SELECT RPT.REPORT_STATUS FROM RMC_RPT_REPORT RPT WHERE RPT.REFERENCE_NO=A.REFERENCE_NO AND RPT.REPORT_TYPE=2 AND RPT.TKH_HAPUS IS NULL " +
            " AND A.PRO_ACTIVATED='CWR'),0) AS STATUS_REPORT " +
            " FROM RMC_PRO A" +
            " LEFT Join RMC_RESMEMBER J On A.REFERENCE_NO = J.REFERENCE_NO And J.TKH_HAPUS Is Null " +
            " LEFT JOIN HR_MAKLUMAT_PERIBADI C ON J.MAKLUMAT_PERIBADI_FK = C.MAKLUMAT_PERIBADI_PK AND A.TKH_HAPUS IS NULL " +
            " LEFT JOIN HR_STAF B ON J.MAKLUMAT_PERIBADI_FK = B.MAKLUMAT_PERIBADI_FK AND B.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_COST_CENTER D on D.REFERENCE_NO = A.REFERENCE_NO AND D.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PTYPEDETAIL F ON F.PTYPEDETAIL_PK = A.PTYPEDETAIL_FK AND F.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PRO_ACTIVATED G ON G.CODE = A.PRO_ACTIVATED AND G.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_RARG H1 ON H1.STAF_FK = B.STAF_PK AND H1.TKH_HAPUS IS NULL AND H1.STATUS_AKTIF = 'Y' " +
            " LEFT JOIN RMC_RESEARCH_ALLIANCE H ON H.PTJ_RA_CODE = H1.PTJ_RA AND H.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PRO2 I ON I.REFERENCE_NO = A.REFERENCE_NO AND I.TKH_HAPUS IS NULL " +
            " WHERE A.TKH_HAPUS IS NULL " +
            " AND J.RESMEMBER_ROLE_FK='77'" +
           " AND B.STAF_PK = '" + user.STAF_FK + "'";


            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string GRANT_PI_ACTIVE = (sb.Append(GRANT_PI).Append("AND A.PSTATUS_ID = 123  AND A.PRO_ACTIVATED in 'A'")).ToString();
            dtblActive = CF.getOracleDT(GRANT_PI_ACTIVE);
            modelDT.dt1 = dtblActive;


            DataTable dtblNotActive = new DataTable();
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            string GRANT_PI_NOTACTIVE = (sb1.Append(GRANT_PI).Append("AND A.PSTATUS_ID = 123 AND A.PRO_ACTIVATED not in 'A'")).ToString();
            dtblNotActive = CF.getOracleDT(GRANT_PI_NOTACTIVE);
            modelDT.dt2 = dtblNotActive;

            DataTable dtblProposal = new DataTable();
            System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
            string GRANT_PROPOSAL = (sb2.Append(GRANT_PI).Append("AND A.PSTATUS_ID NOT IN (123)")).ToString();
            dtblProposal = CF.getOracleDT(GRANT_PROPOSAL);
            modelDT.dt3 = dtblProposal;

            return modelDT;

        }

        public GrantModel GetGrantInfo(string id)
        {
           
            GrantModel model = new GrantModel();
            string GRANT_INFO = "SELECT A.REFERENCE_NO, A.PRO_TITLE, NVL(D.COST_CENTER_CODE,'-') as COST_CENTER_CODE, NVL(G.DESCRIPTION,'-'), " +
            " CASE " +
            "  WHEN H1.PTJ_RA IS NOT NULL " +
            "    THEN H.DESCRIPTION " +
            "  WHEN H1.PTJ_RA IS NULL " +
            "    THEN (SELECT DESKRIPSI FROM HR_JABATAN  WHERE KOD_JABATAN = SUBSTR(B.KOD_PTJ, 0, 5)) " +
            " END, " +
            " (SELECT F.KOD FROM RMC_PTYPEDETAIL F WHERE F.PTYPEDETAIL_PK = A.PTYPEDETAIL_FK) AS GRANT_TYPE, " +
            " DECODE (F.PTYPEDETAIL_CATEGORY,1,'INTERNAL',2,'EXTERNAL') AS GRANT_CATEGORY, " +
            " (SELECT D.NAMA FROM HR_MAKLUMAT_PERIBADI D WHERE D.TKH_HAPUS IS NULL AND D.MAKLUMAT_PERIBADI_PK = A.MAKLUMAT_PERIBADI_FK) AS NAMA, " +
            " (SELECT C.NO_PEKERJA FROM HR_STAF C WHERE C.TKH_HAPUS IS NULL AND C.MAKLUMAT_PERIBADI_FK = A.MAKLUMAT_PERIBADI_FK) AS NO_PEKERJA, " +
            " DECODE ((select NVL(AKUAN_STATUS_FK,0) FROM RMC_PRO WHERE TKH_HAPUS IS NULL AND PRO_ACTIVATED IS NULL AND PSTATUS_ID='123' " +
            "   AND REFERENCE_NO=A.REFERENCE_NO),null,'APPROVED',0,'APPROVED - WAITING COST CENTER',1,'APPROVED - WAITING GENERATE LETTER',204," +
            "   'APPROVED - WAITING FOR ACTIVATION', 205,'APPROVED - WAITING PL DECLARATION', 206,'APPROVED - WAITING PL DECLARATION', 321, 'APPROVED'," +
            "   314,'APPROVED - WAITING FOR ACTIVATION'), " +
            " TO_CHAR(A.TKH_CIPTA,'DD-MM-YYYY') AS TKH_PRO, " +
            " A.PRO_RM, A.SUBMISSION_DATE, A.MAKLUMAT_PERIBADI_FK, A.PSTATUS_ID, G.DESCRIPTION, H.DESCRIPTION, " +
            " A.PTYPEDETAIL_FK, NVL(I.KPI_RU,0), A.AKUAN_STATUS_FK, " +
            " (SELECT MIN (H.END_DATE_PREV) FROM RMC_PM_PINDATEMPOH H WHERE H.TKH_HAPUS IS NULL AND H.REFERENCE_NO = A.REFERENCE_NO) AS DATE_END_PREV, " +
            " H1.PTJ_RA " +
            " ,A.PRO_ACTIVATED " +
            " ,NVL((SELECT RPT.REPORT_STATUS FROM RMC_RPT_REPORT RPT WHERE RPT.REFERENCE_NO=A.REFERENCE_NO AND RPT.REPORT_TYPE=2 AND RPT.TKH_HAPUS IS NULL " +
            " AND A.PRO_ACTIVATED='CWR'),0) AS STATUS_REPORT " +
            " FROM RMC_PRO A" +
            " LEFT Join RMC_RESMEMBER J On A.REFERENCE_NO = J.REFERENCE_NO And J.TKH_HAPUS Is Null " +
            " LEFT JOIN HR_MAKLUMAT_PERIBADI C ON J.MAKLUMAT_PERIBADI_FK = C.MAKLUMAT_PERIBADI_PK AND A.TKH_HAPUS IS NULL " +
            " LEFT JOIN HR_STAF B ON J.MAKLUMAT_PERIBADI_FK = B.MAKLUMAT_PERIBADI_FK AND B.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_COST_CENTER D on D.REFERENCE_NO = A.REFERENCE_NO AND D.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PTYPEDETAIL F ON F.PTYPEDETAIL_PK = A.PTYPEDETAIL_FK AND F.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PRO_ACTIVATED G ON G.CODE = A.PRO_ACTIVATED AND G.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_RARG H1 ON H1.STAF_FK = B.STAF_PK AND H1.TKH_HAPUS IS NULL AND H1.STATUS_AKTIF = 'Y' " +
            " LEFT JOIN RMC_RESEARCH_ALLIANCE H ON H.PTJ_RA_CODE = H1.PTJ_RA AND H.TKH_HAPUS IS NULL " +
            " LEFT JOIN RMC_PRO2 I ON I.REFERENCE_NO = A.REFERENCE_NO AND I.TKH_HAPUS IS NULL " +
            " WHERE A.TKH_HAPUS IS NULL " +
            " AND J.RESMEMBER_ROLE_FK='77'" +
           " AND A.REFERENCE_NO = '" + id + "'";

            var dt = new DataTable();
            dt = CF.getOracleDT(GRANT_INFO);


            GrantModel grantModels = new GrantModel();

            grantModels.REFERENCE_NO = (dt.Rows[0][0]).ToString();
            grantModels.COST_CENTER_CODE = (dt.Rows[0][2]).ToString();
            return grantModels;
        }
    }


}