using HRSystem.Models;

namespace HRSystem.Repository
{
	public interface IAttendanceRepository
	{

        // For Selected Attendance Data
        Attendance GetRecordByID(int id);
        

        
        // For All Attendance Data By Model View
        List<fullAttendanceData> GetAllRecordsInMV(int countOfRecors, int rowNumber);
        int GetCountOfAllRecords();



        // For Attendance Data Filtered By EmpName Of DeptName
        List<fullAttendanceData> GetFilteredRecoredsByEmpName(string searchText, string searchBy, int countOfRecors, int rowNumber);
        int GetCountOfFilteredRecoredsByEmpName(string searchText, string searchBy);



        // For Attendance Data Filtered By Date
        List<fullAttendanceData> GetFilteredRecoredsByDate(DateTime startDate, DateTime endDate, int countOfRecors, int rowNumber);
        int GetCountOfFilteredRecoredsByDate(DateTime startDate, DateTime endDate);



        // For Insert, Update And Delete Attendance Record
        void Insert(Attendance newAttendance);
        void Update(int id, Attendance newAttendance);
        void Delete(int id);
    }
}
