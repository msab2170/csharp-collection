using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .MinimumLevel.Verbose()
    .CreateLogger();

// Console.Write이 아닌 Serilog를 이용했기 때문에
// 이 소스를 그대로 쓰려면 nuget패키지를 다운받아야 한다.(Serilog, Serilog.Sinks.Console 두개 받아야 한다.)


OutputFileSystemInfo();
WorkWithDrives();
MakeandDeleteFolder();


// 이 부분은 설명보다는 직접 한번이라도 돌려보는게 좋을 것 같아 설명은 생략한다.
static void OutputFileSystemInfo(){
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Path.PathSeparator"), Path.PathSeparator);
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Path.DirectorySeparatorChar"), Path.DirectorySeparatorChar);
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Directory.GetCurrentDirectory()"), Directory.GetCurrentDirectory());
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.CurrentDirectory"), Environment.CurrentDirectory);
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.PathSeparator"), Environment.SystemDirectory);
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Path.GetTempPath()"), Path.GetTempPath());
    Log.Debug("{0} {1}", string.Format("{0, -80}","Environment.GetFolderPath(Environment.SpecialFolder.System)"), 
        Environment.GetFolderPath(Environment.SpecialFolder.System));
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)"), 
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)"), 
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)"), 
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
    Log.Debug("{0} {1}", string.Format("{0, -80}", "Environment.GetFolderPath(Environment.SpecialFolder.Personal)"), 
        Environment.GetFolderPath(Environment.SpecialFolder.Personal));
}

// 마찬가지이다.
static void WorkWithDrives()
{
    Log.Debug("{Name,-30} | {Type,-10} | {Format,-7} | {Size,18} | {FreeSpace,18}", 
        "NAME", "TYPE", "FORMAT", "SIZE (BTYES)", "FREE SPACE");
    foreach(DriveInfo drive in DriveInfo.GetDrives()){
        if(drive.IsReady){
            Log.Debug("{Name,-30} | {Type,-10} | {Format,-7} | {Size,18:n} | {FreeSpace,18:n}", 
                drive.Name, drive.DriveType, drive.DriveFormat, drive.TotalSize, drive.AvailableFreeSpace);
        }
        else{
           Log.Debug("{Name,-30} | {Type,-10}", drive.Name, drive.DriveType); 
        }
    }
}
static void MakeandDeleteFolder(){

    // 파일 경로를 쓸때 /나 \를 쓸지말지 고민을 없애준다. 자주 쓰이니 초보자는 암기라도 하자
    // 모든 경우의 수를 생각해서 string로 잘 쓸 수 있다고 주장한다면 말리진 않겠다.
    string newFolder = Path.Combine( 
        Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), 
        "NewFolder"
    );

    // 영어 뜻 그대로다. 해당 경로가 "있으면"에 !를 붙였으니 -> 없으면
    // 나중에 File.Exists()와 FileInfo.Exists도 있다.
    if(!Directory.Exists(path: newFolder)){ 
        // 디렉토리를 생성한다.
        Directory.CreateDirectory(path: newFolder);
    }

    if(Directory.Exists(path: newFolder)){
        // 디렉토리가 있으면 삭제한다. recursive는 true인 경우 하위폴더와 문서까지 모두 삭제한다.
        Directory.Delete(path: newFolder, recursive: true);
    }
}

static void WorWithFiles() {
    string directory = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.Personal),
        "OutputFiles"
    );
    Directory.CreateDirectory(directory);
    Log.Debug("make directory: " + directory);

    string textFile = Path.Combine(directory, "Dummy.txt");
    string backupFile = Path.Combine(directory, "Dummy.bak");

    Console.WriteLine($"textFile:{textFile}, File.Exists(textFile): {File.Exists(textFile)}");
    if(!File.Exists(textFile)){
        StreamWriter textWriter = File.CreateText(textFile);
        textWriter.WriteLine("text file write");
        textWriter.Close();
    }

    File.Copy(sourceFileName: textFile, destFileName: backupFile, overwrite: true);
    Log.Debug($"textFile:{textFile} copy to backupFile: {backupFile}");

    File.Delete(textFile);
    Log.Debug($"textFile:{textFile} deleted.");

    StreamReader textReader = File.OpenText(backupFile);
    Console.WriteLine(textReader.ReadToEnd());
    textReader.Close();

    File.Delete(backupFile);
}
