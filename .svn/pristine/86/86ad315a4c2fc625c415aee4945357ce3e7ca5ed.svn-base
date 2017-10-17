<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParamMaker.aspx.cs" 
    Inherits="CBS.Web.UI.Param.ParamMaker" %>

<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v17.1" Namespace="DevExpress.Web"
    TagPrefix="dxe" %>
<%@ Register Src="ParamMaker_USC.ascx" TagName="ParamMaker_USC" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <link href="../incl/style.css" type="text/css" rel="Stylesheet" />
    <!-- #include file="~/incl/onepost.html" -->
    <!-- #include file="~/incl/uc/UC_Currency.html" -->
    <!-- #include file="~/incl/uc/UC_Number.html" -->
    <!-- #include file="~/incl/uc/UC_Decimal.html" -->
    <style type="text/css">
        .hide
        {
            display: none;
        }
        .pendingDelete
        {
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table class="Box1" width="100%">
            <tr class="H1">
                <td>
                    <asp:Label ID="title" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <dxtc:ASPxPageControl ID="PageCtrl" runat="server" ActiveTabIndex="0" Width="100%">
                        <TabPages>
                            <dxtc:TabPage Text="Existing Parameter">
                                <ContentCollection>
                                    <dxw:ContentControl ID="ContentControl1" runat="server">
                                        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
                                            Width="100%" Font-Size="10px" OnLoad="grid_Load" OnBeforeColumnSortingGrouping="grid_BeforeColumnSortingGrouping"
                                            OnCustomCallback="grid_CustomCallback" KeyFieldName="__KEY">
                                            <Settings ShowFilterRow="True" ShowGroupedColumns="True" ShowGroupPanel="True" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn VisibleIndex="0" Width="1px">
                                                    <ClearFilterButton Visible="True">
                                                    </ClearFilterButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataColumn Caption="Function" VisibleIndex="1" Width="1%">
                                                    <CellStyle Wrap="False">
                                                    </CellStyle>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr class="<%=funcCss %>">
                                                                <td>
                                                                    <input class="Bt1" type="button" value="Expand All" style="width: 100px" onclick="grid.ExpandAll();" />
                                                                </td>
                                                            </tr>
                                                            <tr class="<%=funcCss %>">
                                                                <td>
                                                                    <input class="Bt1" type="button" value="Collapse All" style="width: 100px" onclick="grid.CollapseAll();" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="text-align: center">
                                                                    <input class="Bt1" type="button" value="New" style="width: 100px" runat="server"
                                                                        visible='<%#Request.QueryString["readonly"]==null && Request.QueryString["editonly"]==null%>' onclick="popup$panel$USC_paraminput$clrButton.click();popup.Show()" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <input class="Bt2" type="button" style="width: 80px" 
                                                                        value='<%#(Request.QueryString["readonly"]==null)?"Edit":"View" %>'
                                                                        onclick="<%# "popup.Show();callback(panel,'r:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false)"%>" />
                                                                </td>
                                                                <td>
                                                                    <input class="Bt2" type="button" value="Delete" style="width: 80px" runat="server"
                                                                        visible='<%#Request.QueryString["readonly"]==null && Request.QueryString["editonly"]==null%>' 
                                                                        onclick=<%# "if(confirm('Are you sure want to delete?')){callback(gridpending,'d:" + Container.KeyValue.ToString().Replace("'","\\'")  + "', false)}"%> />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                            </Columns>
                                            <SettingsBehavior AutoFilterRowInputDelay="1000" />
                                            <SettingsPager PageSize="12">
                                            </SettingsPager>
                                        </dxwgv:ASPxGridView>
                                    </dxw:ContentControl>
                                </ContentCollection>
                            </dxtc:TabPage>
                            <dxtc:TabPage Text="Pending Approval">
                                <ContentCollection>
                                    <dxw:ContentControl ID="ContentControl2" runat="server">
                                        <dxwgv:ASPxGridView ID="gridpending" ClientInstanceName="gridpending" runat="server"
                                            AutoGenerateColumns="False" Width="100%" Font-Size="10px" OnLoad="gridpending_Load"
                                            OnBeforeColumnSortingGrouping="gridpending_BeforeColumnSortingGrouping" OnCustomCallback="gridpending_CustomCallback"
                                            KeyFieldName="__KEY">
                                            <Settings ShowFilterRow="True" ShowGroupedColumns="True" ShowGroupPanel="True" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn VisibleIndex="0" Width="1px">
                                                    <ClearFilterButton Visible="True">
                                                    </ClearFilterButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataColumn Caption="Function" VisibleIndex="1" Width="1%">
                                                    <CellStyle Wrap="False">
                                                    </CellStyle>
                                                    <HeaderTemplate>
                                                        <table>
                                                            <tr class="<%=funcpendCss %>">
                                                                <td>
                                                                    <input class="Bt1" type="button" value="Expand All" style="width: 100px" onclick="gridpending.ExpandAll();" />
                                                                </td>
                                                            </tr>
                                                            <tr class="<%=funcpendCss %>">
                                                                <td>
                                                                    <input class="Bt1" type="button" value="Collapse All" style="width: 100px" onclick="gridpending.CollapseAll();" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </HeaderTemplate>
                                                    <DataItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td class="<%# "pending" + Eval("__STATUS").ToString() %>">
                                                                    <input class="Bt1" type="button" value="Edit" style="width: 80px" runat="server"
                                                                        visible='<%#Request.QueryString["readonly"]==null %>' onclick=<%# "popup.Show();callback(panel,'rp:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false)"%> />
                                                                </td>
                                                                <td>
                                                                    <input class="Bt1" type="button" value="Delete" style="width: 80px" runat="server"
                                                                        visible='<%#Request.QueryString["readonly"]==null %>' onclick=<%# "callback(gridpending,'dp:" + Container.KeyValue.ToString().Replace("'","\\'") + "', false)"%> />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </DataItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                            </Columns>
                                            <SettingsBehavior AutoFilterRowInputDelay="1000" />
                                            <SettingsPager PageSize="12">
                                            </SettingsPager>
                                        </dxwgv:ASPxGridView>
                                    </dxw:ContentControl>
                                </ContentCollection>
                            </dxtc:TabPage>
                        </TabPages>
                    </dxtc:ASPxPageControl>
                    <dxpc:ASPxPopupControl ID="popup" ClientInstanceName="popup" runat="server" HeaderText=""
                        Width="90%" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                        CloseAction="CloseButton" Modal="True" AllowDragging="True" EnableAnimation="False">
                        <ContentCollection>
                            <dxpc:PopupControlContentControl ID="PopupControlContentControl1" runat="server"
                                Height="100%">
                                <dxcp:ASPxCallbackPanel ID="panel" runat="server" ClientInstanceName="panel" OnCallback="panel_Callback">
                                    <ClientSideEvents EndCallback="function(s, e){ if(panel.cp_action=='s'){popup.Hide();processing=false;callback(gridpending);}}" />
                                    <PanelCollection>
                                        <dxp:PanelContent ID="PanelContent1" runat="server">
                                            <uc1:ParamMaker_USC ID="USC_paraminput" runat="server" />
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                            </dxpc:PopupControlContentControl>
                        </ContentCollection>
                    </dxpc:ASPxPopupControl>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>