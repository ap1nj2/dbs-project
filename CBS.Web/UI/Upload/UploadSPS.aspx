<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadSPS.aspx.cs" Inherits="CBS.Web.UI.Upload.UploadSPS" %>

<%@ Register Assembly="DevExpress.Web.v17.1, Version=17.1.3.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Import Exsiting SPS and .csv Task 4, 12</h2>
    <p>&nbsp;</p>
    <p><span style="font-size: large">Import Exsiting and .csv Task 4</span></p>
    
    <dx:ASPxUploadControl ID="UploadSPS" runat="server" Theme="MaterialCompact">
        <ValidationSettings AllowedFileExtensions=".xls, .xlsx">
        </ValidationSettings>
    </dx:ASPxUploadControl>
    <br />

    <dx:ASPxButton ID="btnUploadSPS" runat="server" Text="Upload SPS"
        Height="23px" Theme="MaterialCompact" OnClick="btnUploadSPS_Click" />

    <br />
    <br />  
    I<span style="font-size: large">Import Exsiting and .csv Task 12</span><br />
    <br />
    <dx:ASPxButton ID="btnExportCSV" runat="server" OnClick="btnExportCSV_Click" Text="Export to CSV" Theme="MaterialCompact">
    </dx:ASPxButton>
    <br />
</asp:Content>
