<%@ Control Language="C#" CodeBehind="ForeignKey.ascx.cs" Inherits="TekConfAdmin.ForeignKeyFilter" %>

<asp:DropDownList runat="server" ID="DropDownList1" AutoPostBack="True" CssClass="DDFilter"
    OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
    <asp:ListItem Text="Tous" Value="" />
</asp:DropDownList>

