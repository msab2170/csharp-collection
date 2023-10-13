using MsgReader.Mime;
using MsgReader.Outlook;

/*
  MsgReader는 이메일 파일 확장자인 .msg와 .eml을 읽을 수 있도록 하는 라이브러리이다.
  이메일 파일에서 (보낸이, 받는이, 보낸시각, 받은시각, CC, BCC, 메일제목, 메일내용, 첨부파일) 등의 정보를 추출할 수 있다.
  첨부파일의 경우 다운받을 수도 있다.

  특별히 예외처리는 하지 않고 동작가능한부분만 작성했다.
*/

// 메일 제목과 내용은 파일 내 프로퍼티 가져올때 가져올 수 있는데 당장은 생략하도록 하겠다.
// 아래는 임시적으로 보여줄 메일정보 클래스이다. 파일 디렉토리와 네임 FilePath를 다 쓸필요는 없는데 편의상 그렇게 했다.

class MailFile {
    public string FileDirectory { get; set; }          // 파일 경로
    public string FileName { get; set; }               // 파일명
    public string FilePath { get; set; }               // 파일경로 + 파일명
    public string Originator { get; set; }             // 보낸사람
    public string Addressee { get; set; }              // 받는사람
    public string CC { get; set; }                     // CC
    public string BCC { get; set; }                    // BCC
    public DateTime SentDate { get; set; }             //  발신 시간
    public DateTime ReceivedDate { get; set; }         // 수신 시간
    
    public MailFile(string fileDirectory, string fileName)
    {
        FileDirectory = fileDirectory;
        FileName = fileName;
        FilePath = Path.Combine(fileDirectory, fileName);  // 그냥 string으로 더하면 디렉토리와 파일명 사이에 \를 잘 신경써서 처리해줘야하는데 알아서 없으면 붙여주고 있으면 떼주는 기능을 한다.
    }
}

string msgExtention = ".MSG";
string emlExtention = ".EML";

string mailFileDirectory = @"파일경로, \가 보통들어가서 @를 붙임";
string mailFileName = @"파일명";

var mailFile = new MailFile(mailFileDirectory, mailFileName);
// string mailFilePath = mailFile.FilePath; // 여기서 따로 뗄 필요는 없다. 명시적으로 보여주려고 적음

string downloadDirectory = @"첨부파일이있다면 다운받을 경로이다.";

FileInfo mailFileInfo = new FileInfo(mailFilePath);
if (fileInfo.Exists && fileInfo.Length > 0)
{
    if (mailFileInfo.Extension.ToUpper().Equals(msgExtention))
    {
        using var msgFile = new MsgReader.Outlook.Storage.Message(mailFilePath);
        mailFile.Originator = msg.Sender?.DisplayName;                                    // 보낸이
        mailFile.Addressee = msg.GetEmailRecipients(RecipientType.To, false, false);      // 받는이
        mailFile.CC = msg.GetEmailRecipients(RecipientType.Cc, false, false);             // CC
        mailFile.BCC = msg.GetEmailRecipients(RecipientType.Bcc, false, false);           // BCC
        mailFile.SentDate = msg.SentOn ?? DateTime.MinValue;                              // 보낸시각
        mailFile.ReceivedDate = msg.ReceivedOn ?? DateTime.MinValue;                      // 받는시각
    
       var attachmentsObject = msgFile.Attachments;

        attachmentsObject.ForEach(attachment =>
        {
            try
            {
                if (!Directory.Exists(downloadDirectory)){    // 첨부파일을 다운받을 경로가 없으면 만든다.
                    Directory.CreateDirectory(downloadDirectory);
                }
                
                if (attachment != null && attachment is Storage.Attachment)  // 일반적인 첨부파일이면
                {
                    var attachmentCast = attachment as Storage.Attachment ?? throw new Exception($"형변환이 안되면 던질 예");
                    string attachmentPath = Path.Combine(downloadDirectory, attachmentCast.FileName);             // 내려받을 첨부파일 path
                    File.WriteAllBytes(attachmentPath, attachmentCast.Data);                                      // 다운로드           
                }
                else if (attachment != null && attachment is Storage.Message)  // 앞에 using var msgFile = new MsgReader.Outlook.Storage.Message(mailFilePath); 부분을 보면 눈치 챘을 수도 있지만 이는 첨부파일이 MSG파일인 경우이다.
                {
                  var attachmentCast = attachment as Storage.Message ?? throw new Exception($"형변환이 안되면 던질 예외");
                  string attachmentPath = Path.Combine(downloadDirectory, attachmentCast.FileName);
                  attachmentCast.Save(attachmentPath);                       // Storage.Message는 Save함수를 사용하여 파일을 내려받아야 한다.
                }
                else
                {
                  둘다 아닌경우에 취할 조치를 하면 된다. 예외처리를 해도 좋다.
                }
            }
            catch (Exception ex)
            {
              Log.Error(ex.ToString());
            }
      });
    }
    else if (mailFileInfo.Extension.ToUpper().Equals(emlExtention))
    {
        var emlFile = MsgReader.Mime.Message.Load(mailFileInfo);    // eml 파일을 읽는 방법이다.

        if (emlFile.Headers != null)    // eml 파일의 메일 파일정보 불러오기는 복잡하다... msgReader 공식 문서를 참조하면 이렇게 해야한다.
        {
            if (emlFile.Headers.From != null && !string.IsNullOrEmpty(eml.Headers.From.DisplayName))
            { mailFile.Originator = emlFile.Headers.From.DisplayName.ToString().Replace("\"", string.Empty); }
            else if (eml.Headers.From != null && !string.IsNullOrEmpty(eml.Headers.From.Address))
            { mailFile.Originator = emlFile.Headers.From.Address.ToString().Replace("\"", string.Empty); }
            else { mailFile.Originator = ""; }
        
            emlFile.Headers.To.ForEach(to => mailInfo.Addressee += "," + to.ToString());
            if (mailFile.Addressee == null) { mailFile.Addressee = ""; }
            else { mailFile.Addressee = mailFile.Addressee.Substring(1).Replace("\"", string.Empty); }
        
            emlFile.Headers.Cc.ForEach(cc => mailFile.CC += "," + cc.ToString());
            if (mailFile.CC == null) { mailFile.CC = ""; }
            else { mailFile.CC = mailFile.CC.Substring(1).Replace("\"", string.Empty); }
        
            emlFile.Headers.Bcc.ForEach(bcc => mailFile.BCC += "," + bcc.ToString());
            if (mailFile.BCC == null) { mailFile.BCC = ""; }
            else { mailFile.BCC = mailInfo.BCC.Substring(1).Replace("\"", string.Empty); }
        
            if (emlFile.Headers.DateSent != null) { mailFile.SentDate = emlFile.Headers.DateSent; }
            else { mailFile.SentDate = DateTime.MinValue; }
        
            if (emlFile.Headers.Received != null && emlFile.Headers.Received.Count > 0)
            {
                mailFile.ReceivedDate = emlFile.Headers.Received.Last().Date;
            }
            else
            {
                mailFile.ReceivedDate = DateTime.MinValue;
            }
        }    // 여기까지가 메일정보 읽기
        
        ObservableCollection<MessagePart> attachments = emlFile.Attachments;    
        var attachmentsCount = attachments.Count;       
    
        foreach (var attachment in attachments) // 다만 첨부파일 다운로드는 간단하다.
        { 
            try
            {
                if (!Directory.Exists(downloadDirectory)) 
                    Directory.CreateDirectory(downloadDirectory);
    
                if (attachment.IsAttachment)
                    attachment.Save(new FileInfo(Path.Combine(downloadDirectory, attachment.FileName)));             
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
