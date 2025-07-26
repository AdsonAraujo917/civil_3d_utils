
public class Xdata
{
    private static ResultBuffer insertOldXData(string appName, ResultBuffer newResultBuffer, DBObject dbObject)
    {
        ResultBuffer oldResultBuffer = dbObject.GetXDataForApplication(appName);
        if (oldResultBuffer != null)
        {
            foreach (TypedValue typedValue in oldResultBuffer)
            {
                if (typedValue.TypeCode != (int)DxfCode.ExtendedDataRegAppName)
                {
                    newResultBuffer.Add(new TypedValue((int)typedValue.TypeCode, typedValue.Value));
                }
            }
        }
        return newResultBuffer;
    }
    
    private static bool createRegAppTableRecord(string appName, Transaction transaction, Database database)
    {
        RegAppTable regAppTable = (RegAppTable)transaction.GetObject(database.RegAppTableId, OpenMode.ForRead);
        if (!regAppTable.Has(appName))
        {
            using (RegAppTableRecord regAppTableRecord = new RegAppTableRecord())
            {
                regAppTable.UpgradeOpen();
                regAppTableRecord.Name = appName;
                transaction.GetObject(database.RegAppTableId, OpenMode.ForWrite);
                regAppTable.Add(regAppTableRecord);
                transaction.AddNewlyCreatedDBObject(regAppTableRecord, true);
            }
        }
        return true;
    }

    public static void SetXData(string appName, int value, Database database, Transaction transaction, DBObject dbObject)
    {
        Xdata.createRegAppTableRecord(appName, transaction, dbObject.Database);
        using (ResultBuffer resultBuffer = new ResultBuffer())
        {
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, appName));
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataInteger32, value));
            dbObject.XData = resultBuffer;
        }
    }

    public static void SetXData(string appName, List<Tuple<string, dynamic, int>> data, Transaction transaction, DBObject dbObject)
    {
        Database db = dbObject.Database;
        ResultBuffer resultbuffer = new ResultBuffer(new TypedValue((int)DxfCode.ExtendedDataRegAppName, appName));
        Xdata.createRegAppTableRecord(appName, transaction, dbObject.Database);
        using (ResultBuffer resultBuffer = new ResultBuffer())
        {
            foreach (Tuple<string, dynamic, int> item in data)
            {
                resultbuffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, item.Item1));
                resultbuffer.Add(new TypedValue(item.Item3, item.Item2));
            }
            dbObject.XData = resultbuffer;
            resultbuffer.Dispose();
        }
    }

    public static void SetXData(string appName, Handle value, Database database, Transaction transaction, DBObject dbObject)
    {
        Xdata.createRegAppTableRecord(appName, transaction, dbObject.Database);
        using (ResultBuffer resultBuffer = new ResultBuffer())
        {
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, appName));
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataHandle, value));
            dbObject.XData = resultBuffer;
        }
    }

    public static void SetXData(string appName, string value, Database database, Transaction transaction, DBObject dbObject)
    {
        Xdata.createRegAppTableRecord(appName, transaction, dbObject.Database);
        using (ResultBuffer resultBuffer = new ResultBuffer())
        {
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, appName));
            resultBuffer.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, value));
            dbObject.XData = resultBuffer;
        }
    }

    public static String[] GetXData(string appName, DBObject dbObject) 
    {
        ResultBuffer resultBuffer = dbObject.GetXDataForApplication(appName);
        List<String> stringList = new List<string> { };
        foreach (TypedValue typedValue in resultBuffer.AsArray()) {
            stringList.Add(typedValue.Value.ToString());
        }
        return stringList.ToArray();
    }
}

