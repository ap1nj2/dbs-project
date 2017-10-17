<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamMaker_USC.ascx.cs"
    Inherits="CBS.Web.UI.Param.ParamMaker_USC" %>
<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.3.0, Culture=neutral"
    Namespace="DevExpress.Web" TagPrefix="dxe" %>
<%@ Register Src="../UserControl/USC_PopUp_Cascade.ascx" TagName="USC_PopUp_Cascade"
    TagPrefix="uc1" %>
<input id="__hSTATUS" type="hidden" runat="server" />
<uc1:USC_PopUp_Cascade ID="ucpuc" runat="server" />
<table class="Box1" width="100%">
    <tr>
        <td align="center">
            <pre dir="ltr" style="margin: 0px; padding: 2px; border: 1px inset; width: 100%;
                height: 300px; text-align: left; overflow: auto">
                <table id="TableInput" runat="server" width="100%">
                </table>
            </pre>
        </td>
    </tr>
    <tr>
        <td runat="server" id="td_filter" align="center">
            <input id="btnSave" runat="server" class="Bt1" type="button" visible='<%#Request.QueryString["readonly"]==null %>'
                value=" Save " onclick="callback(panel,'s:')" />
            <input id="clrButton" runat="server" class="Bt1" type="button" value=" Clear " style="display: none" />
            <input id="Button2" runat="server" class="Bt1" type="button" value=" Cancel " onclick="popup.Hide();" />
        </td>
    </tr>
</table>