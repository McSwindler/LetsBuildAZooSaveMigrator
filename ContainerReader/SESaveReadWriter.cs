using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;

namespace ContainerReader
{
  public static class SESaveReadWriter
  {

    public static void WriteFile(string filename, string content, bool encrypt)
    {
        dynamic reader = GetReader(filename, content, encrypt);
        if(reader != null)
        {
            Write(reader);
        }
    }

    public static void SetIgnoreKeysForSave(Assembly DLL, bool value)
    {
        DLL.GetType("SEngine.FlagSettings").GetField("IgnoreKeysForSave", BindingFlags.Public | BindingFlags.Static).SetValue(null, value);
    }
    
    public static dynamic GetReader(string filename, string content, bool encrypt)
    {
        string strDllPath = Path.GetFullPath("SEngine.dll");
        if (File.Exists(strDllPath))
        {
            // Execute the method from the requested .dll using reflection (System.Reflection).
            Assembly DLL = Assembly.LoadFrom(strDllPath);
            SetIgnoreKeysForSave(DLL, true);
            Type classType = DLL.GetType(String.Format("{0}.{1}", "SEngine", "Reader"));
            if (classType != null)
            {
                // Create class instance.
                dynamic classInst = Activator.CreateInstance(classType);

                MethodInfo methodInfo = classType.GetMethod("FromString", new Type[]{typeof(string)});
                if (methodInfo != null)
                {
                    methodInfo.Invoke(classInst, new object[]{content});
                }

                SetFieldValue(classInst, "m_isEncryptionDecryptionEnabled", encrypt);
                SetFieldValue(classInst, "m_originalFileName", filename);

                return classInst;
            }
        }
        return null;
    }

    private static void SetFieldValue(object classInst, string fieldName, object value)
    {
        FieldInfo fieldInfo = classInst.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        if (fieldInfo != null)
        {
            fieldInfo.SetValue(classInst, value);
        }
    }

    public static void Write(dynamic reader)
    {
        string strDllPath = Path.GetFullPath("SEngine.dll");
        if (File.Exists(strDllPath))
        {
            // Execute the method from the requested .dll using reflection (System.Reflection).
            Assembly DLL = Assembly.LoadFrom(strDllPath);
            SetIgnoreKeysForSave(DLL, false);
            Type classType = DLL.GetType(String.Format("{0}.{1}", "SEngine", "Writer"));
            if (classType != null)
            {
                // Create class instance.
                dynamic classInst = Activator.CreateInstance(classType, new Object[]{reader});
                classInst.Close();
            }
        }
    }
  }
}