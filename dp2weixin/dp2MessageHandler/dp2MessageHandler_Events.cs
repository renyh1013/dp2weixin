﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using Senparc.Weixin.Context;
using dp2Command.Server;
namespace dp2weixin
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class dp2MessageHandler : MessageHandler<dp2MessageContext>
    {
        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            string strMessage = "您好，欢迎关注数字平台公众号！您可以回复：\n"
               + "search:检索" + "\n"
               + "binding:绑定读者账号" + "\n"
               + "unbinding:解除绑定" + "\n"
               + "myinfo:个人信息" + "\n"
               + "borrowinfo:借阅信息" + "\n"
               + "renew:续借" + "\n"
               + "bookrecommend:新书推荐" + "\n"
               + "notice:最新公告";
            return this.CreateTextResponseMessage(strMessage,false);
        }

        /// <summary>
        /// 自定义菜单点击事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            //菜单点击，需要跟创建菜单时的Key匹配
            // 注意这里为了与命令常量一致，都转成小写了
            string strEventKey = requestMessage.EventKey.ToLower();

            //设当前命令路径，用于在回复时输出
            this.CurrentMessageContext.CurrentCmdPath = strEventKey;

            switch (strEventKey) 
            {
                case dp2CommandUtility.C_Command_Search: //"Search":
                    {
                        return this.DoSearch("");
                    }
                case dp2CommandUtility.C_Command_Binding://"Binding":
                    {
                        return this.DoBinding("");
                    }
                case dp2CommandUtility.C_Command_Unbinding://"Unbinding":
                    {
                        return this.DoUnbinding();
                    }
                case dp2CommandUtility.C_Command_MyInfo://"MyInfo":
                    {
                        return this.DoMyInfo();
                    }
                case dp2CommandUtility.C_Command_BorrowInfo:// "BorrowInfo":
                    {
                        return DoBorrowInfo();
                    }
                case dp2CommandUtility.C_Command_Renew:// "Renew":
                    {
                        return DoRenew("");
                    }
                case dp2CommandUtility.C_Command_BookRecommend://"BookRecommend":
                    {
                        return this.DoNewBooks();
                    }
                case dp2CommandUtility.C_Command_Notice:
                    {
                        return this.DoNotice();
                    }
                default:
                    {
                        return this.CreateTextResponseMessage("未知的命令:" + requestMessage.EventKey);
                    }
            }
        }

        #region 其它用户事件消息

        /// <summary>
        /// 微信客户端（通过微信服务器）自动发送过来的位置信息事件
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这里写什么都无所谓，比如：上帝爱你！";
            return responseMessage;
        }
        /// <summary>
        /// 退订;
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// unsubscribe事件的意义在于及时删除网站应用中已经记录的OpenID绑定，消除冗余数据。并且关注用户流失的情况。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            return responseMessage;
        }

        /// <summary>
        /// 扫描带参数二维码事件
        /// 实际上用户无法收到非订阅账号的消息，所以这里可以随便写。
        /// scan事件的意义在于获取扫描二维码中包含的参数，便于识别和统计用户扫描了哪个二维码。
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "感谢扫码";
            return responseMessage;
        }

        #endregion
    }
}