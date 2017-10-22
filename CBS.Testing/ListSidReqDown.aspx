<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListSidReqDown.aspx.cs" Inherits="CBS.Testing.ListSidReqDown" %>

<%@ Register Assembly="DevExpress.Web.v12.2" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v12.2" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v12.2" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v12.2" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <input type="hidden" runat="server" id="h_ValProcess" />
            <input type="hidden" runat="server" id="h_Val" />
            <input type="hidden" runat="server" id="h_ValPrior" />
            <input type="hidden" runat="server" id="h_selectPriorityAll" />
            <input type="hidden" runat="server" id="h_selectProcessAll" />
            <input type="hidden" runat="server" id="h_selectPriority" />
            <input type="hidden" runat="server" id="hAPPRV" />
            <input type="hidden" runat="server" id="hAPPRVPrior" />
            <input type="hidden" runat="server" id="h_disabled" />
            <input type="hidden" runat="server" id="h_disabledProc" />
            <input type="hidden" runat="server" id="h_reqdisabled" />
            <input type="hidden" runat="server" id="h_haspriority" />
            <input type="hidden" runat="server" id="h_haspriorityEnabled" />
            <input type="hidden" runat="server" id="h_pageSize" />
            <table width="100%" class="Box1">
                <tr>
                    <td>
                        <table width="100%">
                            <tr class="H1" style="display: none">
                                <td>SID / BI Checking
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
