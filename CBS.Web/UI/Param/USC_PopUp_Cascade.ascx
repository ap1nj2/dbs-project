<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="USC_PopUp_Cascade.ascx.cs" 
    Inherits="CBS.Web.UI.Param.USC_PopUp_Cascade" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.3.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dxwgv" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.3.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dxpc" %>
<%@ Register assembly="DevExpress.Web.v17.1, Version=17.1.3.0, Culture=neutral" namespace="DevExpress.Web" tagprefix="dxe" %>





<dxpc:ASPxPopupControl ID="Popup" runat="server" ShowHeader="False">
<ContentCollection>
<dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
    <dxwgv:ASPxGridView ID="Grid" runat="server" OnLoad="Grid_Load" 
        AutoGenerateColumns="False"  
        OnAfterPerformCallback="Grid_AfterPerformCallback" 
        OnCustomCallback="Grid_CustomCallback">
        <SettingsBehavior AutoFilterRowInputDelay="2000"  />
        <settings showfilterrow="True" />
        <Styles>
            <AlternatingRow Enabled="True">
            </AlternatingRow>
        </Styles>
    </dxwgv:ASPxGridView>
    <input id="oCascade" runat="server" type="hidden" />           
</dxpc:PopupControlContentControl>
</ContentCollection>
</dxpc:ASPxPopupControl>

    

        

    

        

    

    

        

    

        

    

    

        

    

        

    
</dxpc:PopupControlContentControl>
</ContentCollection>
</dxpc:ASPxPopupControl>        



