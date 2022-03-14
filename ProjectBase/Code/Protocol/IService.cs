﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntryEngine.Network;

/// <summary>不需要登录的通用服务接口</summary>
[ProtocolStub(1, null)]
public interface IService
{
    #region 用户登录

    /// <summary>发送短信验证码</summary>
    /// <param name="telphone">手机号码</param>
    /// <param name="callback">再次发送冷却时间（秒）</param>
    void SendSMSCode(string telphone, Action<int> callback);

    /// <summary>手机验证码登录，没有账号则注册账号</summary>
    /// <param name="telphone">手机号</param>
    /// <param name="code">验证码</param>
    /// <param name="callback">返回账号信息</param>
    void LoginBySMSCode(string telphone, string code, Action<T_USER> callback);
    /// <summary>Token登录</summary>
    void LoginByToken(string token, Action<T_USER> callback);
    /// <summary>密码登录</summary>
    void LoginByPassword(string telphone, string password, Action<T_USER> callback);
    /// <summary>忘记密码：手机号和验证码登录，并修改密码</summary>
    void ForgetPassword(string telphone, string code, string password, Action<T_USER> callback);

    #endregion

    #region 后台登录

    void CenterLoginByPassword(string name, string password, Action<T_CENTER_USER> callback);
    void CenterLoginBySMSCode(string telphone, string code, Action<T_CENTER_USER> callback);

    #endregion

    #region 上传文件

    void UploadImage(FileUpload file, Action<string> callback);
    void UploadFile(FileUpload file, Action<string> callback);

    #endregion

    #region 支付回调

    void WeChatPayCallback();
    /// <summary>支付宝支付回调</summary>
    void AlipayCallback(
        string trade_no, string out_trade_no,
        string buyer_id, string buyer_logon_id,
        string trade_status, string total_amount, string gmt_payment,
        Action<string> callback);

    #endregion
}
