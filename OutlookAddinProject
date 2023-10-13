using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using Office = Microsoft.Office.Core;

/*
  여기에 적혀있는 것들은 대부분 outlookAddIn 프로젝트를 만들면 들어있는 내용이다.
  이메일을 보냈을때, 특정 로직을 수행하는 방법을 외워두기는 자주쓰는 내용은 아니고, 찾기는 부정확한 정보도 많은 것 같아 정리했다.
*/
namespace 네임스페이스명
{
    public partial class ThisAddIn
    {

        /// <summary>
        /// Outlook이 실행될때 ThisAddIn_Startup가 작동한다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            다른, outlook 실행시 시작되어야 할 내용들을 추가해도 된다.

            // 이 예제에서는 "이메일 보낼때"를 감지하는 이벤트에 이메일 보낼때 실행시킬 메소드를 부착한다.
            this.Application.ItemSend += new Outlook.ApplicationEvents_11_ItemSendEventHandler(Application_ItemSend);
        }

        /// <summary>
        /// 이메일 보낼때 실행할 메소드
        /// </summary>
        /// <param name="Item"></param>
        /// <param name="Cancel"></param>
        private void Application_ItemSend(object Item, ref bool Cancel)
        {
            try
            {
                if (Item is Outlook.MailItem)  // MailItem이 맞으면 메일정보(첨부파일관련 포함)를 읽어오는 등의 로직을 수행할 수 있다. 
                {
                    Outlook.MailItem email = Item as Outlook.MailItem;

                    // 보낸이의 이메일 자체를 가져오는 작업은 복잡하고 MS공식페이지에도 있어 적어두었다.
                    string sender;
                    if (GetSenderSMTPAddress(email) != null)    sender = GetSenderSMTPAddress(email);          
                    else                                        sender = email.SendUsingAccount.SmtpAddress;    
                    //

                }
            }
            catch (System.Exception ex)
            {
                string error = $"{nameof(Application_ItemSend)} - {ex.ToString()}";  
            }    
        }
        
        /// <summary>
        /// MS 공식 홈페이지에 적힌 함수를 그대로 가져온 것이다. 링크 : https://learn.microsoft.com/en-us/office/client-developer/outlook/pia/how-to-get-the-smtp-address-of-the-sender-of-a-mail-item
        /// sender를 가져온다. 특정 조건에서는 이 함수로 읽지 못할 수 있음
        /// </summary>
        /// <param name="mail"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private string GetSenderSMTPAddress(Outlook.MailItem mail)
        {
            string PR_SMTP_ADDRESS = @"http://schemas.microsoft.com/mapi/proptag/0x39FE001E";
            if (mail == null)   throw new ArgumentNullException();
            
            if (mail.SenderEmailType == "EX")
            {
                Outlook.AddressEntry sender = mail.Sender;
                if (sender != null)
                {
                    //Now we have an AddressEntry representing the Sender
                    if (sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeUserAddressEntry
                        || sender.AddressEntryUserType == Outlook.OlAddressEntryUserType.olExchangeRemoteUserAddressEntry)
                    {
                        //Use the ExchangeUser object PrimarySMTPAddress
                        Outlook.ExchangeUser exchUser = sender.GetExchangeUser();
        
                        if (exchUser != null)    return exchUser.PrimarySmtpAddress;                    
                        else                        return null;
                    }
                    else
                    {
                        return sender.PropertyAccessor.GetProperty(PR_SMTP_ADDRESS) as string;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return mail.SenderEmailAddress;
            }
        }


        // 이 아래 메소드들은 모두 OutlookAddin 프로젝트 생성시 자동생성되는 부분이다.

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // 참고: Outlook은 더 이상 이 이벤트를 발생시키지 않습니다. Outlook이 종료될 때 
            //    실행해야 하는 코드가 있으면 웹 사이트(https://go.microsoft.com/fwlink/?LinkId=506785)를 참조하세요.
        }

        #region VSTO에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        
        #endregion
    }
}
