<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="Configuration.aspx.vb" Inherits="Configuration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <style type="text/css">
        .auto-style1 {
            font-family: "Segoe UI";
            font-size: large;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <span class="auto-style1">Configuration</span><asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" DataSourceID="SqlDataSource1">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name" />
            <asp:BoundField DataField="Value" HeaderText="Value" SortExpression="Value" />
            <asp:BoundField DataField="StoreId" HeaderText="StoreId" SortExpression="StoreId" />
            <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
            <asp:BoundField DataField="DateTimeStamp" HeaderText="DateTimeStamp" SortExpression="DateTimeStamp" />
            <asp:CommandField ShowEditButton="True" />
            <asp:CommandField ShowDeleteButton="false" />
            <asp:CommandField ShowSelectButton="false" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" DeleteCommand="DELETE FROM [Configuration] WHERE [Id] = @Id" InsertCommand="INSERT INTO [Configuration] ([Name], [Value], [StoreId], [DateTimeStamp]) VALUES (@Name, @Value, @StoreId, @DateTimeStamp)" SelectCommand="SELECT [Name], [Value], [StoreId], [Id], [DateTimeStamp] FROM [Configuration]" UpdateCommand="UPDATE [Configuration] SET [Name] = @Name, [Value] = @Value, [StoreId] = @StoreId, [DateTimeStamp] = @DateTimeStamp WHERE [Id] = @Id">
        <DeleteParameters>
            <asp:Parameter Name="Id" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Value" Type="String" />
            <asp:Parameter Name="StoreId" Type="Int32" />
            <asp:Parameter Name="DateTimeStamp" Type="DateTime" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Name" Type="String" />
            <asp:Parameter Name="Value" Type="String" />
            <asp:Parameter Name="StoreId" Type="Int32" />
            <asp:Parameter Name="DateTimeStamp" Type="DateTime" />
            <asp:Parameter Name="Id" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderRightCol" Runat="Server">
</asp:Content>

