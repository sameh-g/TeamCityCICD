using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Excel;

namespace SamehAutomation
{
    public class TestCasesParser
    {
        static string TestDataFile { get; set; }
        static Dictionary<string, WorksheetData> _parsedTestData;

        public class WorksheetData
        {
            //method name to access the first dictionary, then key to get the value of the column in this row
            public Dictionary<string, Dictionary<string, string>> MethodsInSheet;
            public int MethodsCount;
        }

        public static WorksheetData GetSheetData(string worksheetName)
        {
            if (_parsedTestData[worksheetName] != null)
                return _parsedTestData[worksheetName];
            else
                return null;

        }//end method

        public static void Init(string testDataFilePath)
        {
            //This dictionary holds the parsed worksheets, so the sheet is only parsed once
            _parsedTestData = new Dictionary<string, WorksheetData>(); //new Dictionary<string, Dictionary<string, string>>();
             TestDataFile = testDataFilePath;

            //Parse
            ParseExcelTestData();
        }

        public static void ParseExcelTestData()
        {
            #region Open test data file for reading
            FileStream stream = File.Open(TestDataFile, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            stream.Dispose();
            excelReader.IsFirstRowAsColumnNames = true;
            #endregion

            int sheetsCount = excelReader.ResultsCount;

            //This step transforms each worksheet to a dataset
            for (int counter = 0; counter < sheetsCount; counter++)
            {
                WorksheetData worksheet = new WorksheetData();
                worksheet.MethodsInSheet = new Dictionary<string, Dictionary<string, string>>();


                DataTable requiredWorkSheet = excelReader.AsDataSet().Tables[counter];
                string sheetName = excelReader.AsDataSet().Tables[counter].TableName;
                worksheet.MethodsCount = requiredWorkSheet.Rows.Count - 1;

                //The actual test data starts from the 10th row in the excel sheet (The above rows are for test reference)
                //Due to the setting that First Row As Coulmn Names, the data is considered to start from the 9th row 
                //As we are zero-based, the index is 8
                for (int rowCounter = 1; rowCounter < requiredWorkSheet.Rows.Count; rowCounter++)
                {
                    //The method name is the fifth column in the sheet (column index is 4 for zero-based array)
                    string inspectedMethodName = requiredWorkSheet.Rows[rowCounter].ItemArray[2].ToString();

                    //there is a method
                    if (inspectedMethodName != string.Empty)
                    {
                        //Create new dictionary for this method that holds the values of its columns (parameters AKA test data)
                        Dictionary<string, string> testCaseData = new Dictionary<string, string>();

                        //if (inspectedMethodName == requiredMethodName)
                        {
                            //The actual data starts from the 5th column, index is 4
                            for (int coulmnCounter = 2; coulmnCounter < requiredWorkSheet.Columns.Count; coulmnCounter++)
                            {
                                if (requiredWorkSheet.Columns[coulmnCounter].ColumnName != string.Empty)
                                {
                                    //if this cell is not empty (it has data in it)
                                    if (requiredWorkSheet.Rows[rowCounter].ItemArray[coulmnCounter].ToString() != string.Empty)
                                    {
                                        //Add the value of this cell to the Data Dictionary
                                        // Key is the Column Header (Control Name), the value is the value located in the cell
                                        string keyColumn = requiredWorkSheet.Columns[coulmnCounter].ColumnName;
                                        string valueInRow = requiredWorkSheet.Rows[rowCounter].ItemArray[coulmnCounter].ToString();

                                        testCaseData.Add(keyColumn, valueInRow);

                                    }//enndif

                                }//if this column has a name: any column without a name will not be added

                            }//endfor

                            //exit the loop as you already found the required method
                            //so there is no need to loop on the rest of the rows
                            //break;

                        }//endif

                        worksheet.MethodsInSheet.Add(inspectedMethodName, testCaseData);

                    }//if there is a method here

                }//endfor


                _parsedTestData.Add(sheetName, worksheet);

            }//end for


        }//end method ParseTestDataFromExcel

        //GetValueOf("columnname", "rowname", "sheetname");
        public static string GetValueOf(string fieldName, string testCaseName, string sheetName)
        {
            string value = null;
            try
            {
                value = _parsedTestData[sheetName].MethodsInSheet[testCaseName][fieldName];
            }
            catch (Exception ex)
            {
                //TODO: logg this exception
                
                return ex.StackTrace;
            }

            return value;

        } //end method

        public static List<string> GetAllStringValuesOf(string key, string methodName, string sheetName, char delimiter = ',')
        {
            try
            {
                List<string> splittedTestData = new List<string>();

                if (_parsedTestData[sheetName].MethodsInSheet[methodName][key] != null)
                {
                    string commaSeparatedTestData = _parsedTestData[sheetName].MethodsInSheet[methodName][key];
                    string[] dataElements = commaSeparatedTestData.Split(delimiter);

                    foreach (string element in dataElements)
                    {
                        splittedTestData.Add(element.Trim());
                    }

                } //endif

                return splittedTestData;

            }
            catch (Exception ex)
            {
                //Logger.Log(ex.Message);
                return null;
            }

        } //end method


        public static ArrayList GetAllValuesOf(string key, string methodName, string sheetName, char delimiter = ',')
        {
            try
            {
                ArrayList splittedTestData = new ArrayList();
                //var testData = TestCasesParser.ParseTestDataFromExcel(className, methodName);

                if (_parsedTestData[sheetName].MethodsInSheet[methodName][key] != null)
                {
                    string commaSeparatedTestData = _parsedTestData[sheetName].MethodsInSheet[methodName][key];
                    string[] dataElements = commaSeparatedTestData.Split(delimiter);

                    foreach (string element in dataElements)
                    {
                        splittedTestData.Add(element.Trim());
                    }

                }//endif

                return splittedTestData;
            }
            catch (Exception ex)
            {
                //Logger.Log(ex.Message);
                return null;
            }

        }//end method

    }//end class
}
