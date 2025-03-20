<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="PurchaseProductsList.aspx.vb" Inherits="PurchaseProductsList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div style="visibility:visible">
             <asp:DropDownList ID="DDL_SelectYears" runat="server"   ViewStateMode="Disabled"  Visible="true">
        <asp:ListItem Value="2017" Text="2017"></asp:ListItem>
		<asp:ListItem Value="2016" Text="2016"></asp:ListItem>
        <asp:ListItem Value="2015" Text="2015"></asp:ListItem>
        <asp:ListItem Value="2014" Text="2014"></asp:ListItem>
        <asp:ListItem Value="2013" Text="2013"></asp:ListItem>
        <asp:ListItem Value="2012" Text="2012"></asp:ListItem>
        <asp:ListItem Value="2011" Text="2011"></asp:ListItem>
        <asp:ListItem Value="2010" Text="2010"></asp:ListItem>
        <asp:ListItem Value="2009" Text="2009"></asp:ListItem>
        <asp:ListItem Value="2008" Text="2008"></asp:ListItem>
        <asp:ListItem Value="2007" Text="2007"></asp:ListItem>
        <asp:ListItem Value="2006" Text="2006"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="SelectMonth" runat="server"    ViewStateMode="Disabled" Visible="true">
        <asp:ListItem Value="1" >Jan</asp:ListItem>
        <asp:ListItem Value="2">Feb</asp:ListItem>
        <asp:ListItem Value="3">Mar</asp:ListItem>
        <asp:ListItem Value="4">Apr</asp:ListItem>
        <asp:ListItem Value="5">May</asp:ListItem>
        <asp:ListItem Value="6">Jun</asp:ListItem>
        <asp:ListItem Value="7">Jul</asp:ListItem>
        <asp:ListItem Value="8">Aug</asp:ListItem>
        <asp:ListItem Value="9">Sep</asp:ListItem>
        <asp:ListItem Value="10">Oct</asp:ListItem>
        <asp:ListItem Value="11">Nov</asp:ListItem>
        <asp:ListItem Value="12">Dec</asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DropDownList2" runat="server"  ViewStateMode="Disabled"  Visible="false" >
        <asp:ListItem Value ="1" Text="Hide_Edited"></asp:ListItem>
        <asp:ListItem Value ="0" Text="Show_Edited"></asp:ListItem>
    </asp:DropDownList>
    <asp:DropDownList ID="DDL_Quarters" runat="server" Visible="false">
        <asp:ListItem Value="1">First Quarter</asp:ListItem>
        <asp:ListItem Value="2">Second Quarter</asp:ListItem>
        <asp:ListItem Value="3">Third Quarter</asp:ListItem>
        <asp:ListItem Value="4">Fourth Quarter</asp:ListItem>
    </asp:DropDownList>
     <asp:DropDownList ID="DDL_Gender" runat="server"  Visible="true" DataSourceID="SqlDataSource_GenderList" DataTextField="Alt_Gender" DataValueField="Alt_Gender" AppendDataBoundItems="true" >
                 <asp:ListItem Value="0">ALL Genders</asp:ListItem>
     </asp:DropDownList>
     <asp:DropDownList ID="DDL_ChildAdult" runat="server"  Visible="true" AppendDataBoundItems="true" >
                <asp:ListItem Value="999">Child & Adult</asp:ListItem>
                <asp:ListItem Value="0">Child</asp:ListItem>
                <asp:ListItem Value="20">Adult</asp:ListItem>
     </asp:DropDownList>
             <asp:SqlDataSource ID="SqlDataSource_GenderList" runat="server" ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
                 SelectCommand="SELECT DISTINCT Alt_Gender FROM GenderMaping"></asp:SqlDataSource>
     <asp:DropDownList ID="ddlDay" runat="server"  Visible="false">
     </asp:DropDownList>
    <asp:Label ID="lblExchangerate" runat="server" Text=""></asp:Label>
    <asp:TextBox ID="tb_Exchangerate" runat="server" Width="33px" Visible ="false" Text =".86" ></asp:TextBox>
     <asp:DropDownList ID="DDL_Top500" runat="server" Visible="true">
         <asp:ListItem Value="0">ALL</asp:ListItem>
         <asp:ListItem Value="1">Top 500</asp:ListItem>
     </asp:DropDownList>
	<asp:DropDownList ID="DLLCountry" runat="server" DataSourceID="SqlDataSource_countries" 
        DataTextField="name" DataValueField="name"  AppendDataBoundItems="True"  Visible="True">
        <asp:ListItem Value="ALL">All Countries</asp:ListItem>
        <asp:ListItem Value="EU">EU Countries</asp:ListItem>
        <asp:ListItem Value="NONE_EU">None EU Countries</asp:ListItem>
       <asp:ListItem Value="UK">Under UK</asp:ListItem> 
        <asp:ListItem Value="0">-------------</asp:ListItem>
    </asp:DropDownList>
             <asp:DropDownList ID="DDL_Catagories_list" runat="server" 
                 DataSourceID="SqlDataSource2" DataTextField="Category_Name" 
                 DataValueField="Category_ID" AppendDataBoundItems="True" >
                 <asp:ListItem Value="0">ALL Catagories</asp:ListItem>
             </asp:DropDownList>
             <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                 ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
                 SelectCommand="SELECT Category_ID, Category_Name FROM Categories WHERE (Hidden = 0) ORDER BY Category_Name">
             </asp:SqlDataSource>
             <asp:SqlDataSource ID="SqlDataSource_countries" runat="server" ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
                 SelectCommand="SELECT iso, name, printable_name, iso3, numcode, Shipping_Zone, SSMA_TimeStamp, RoyalMailCountyCode, id, Postcode_pattern_id, DateApplied, VATRate, VATRate1, VATRate2, VATRate3, VATApplicable, country_id, VAT_ThresholdLimit, Region 
                 FROM Country">
			</asp:SqlDataSource>
             <asp:DropDownList ID="DDL_Suppliers_list" runat="server" 
                 DataSourceID="SqlDataSource_Suppliers" DataTextField="Supplier" 
                 DataValueField="Sup_id" AppendDataBoundItems="true" >
                 <asp:ListItem Value="0">ALL Suppliers</asp:ListItem>
             </asp:DropDownList>
             <asp:SqlDataSource ID="SqlDataSource_Suppliers" runat="server" 
                 ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
				SelectCommand="SELECT DISTINCT p.Supplier_ID AS Sup_id, s.Supplier + ' (' + CONVERT (varchar(4), COUNT(p.Product_ID)) + ')' AS Supplier FROM Products AS p LEFT OUTER JOIN Suppliers AS s ON s.Sup_ID = p.Supplier_ID WHERE (p.Product_Type = 'sale') AND (ISNULL(p.Supplier_ID, '') &lt;&gt; '') AND (ISNULL(p.Product_SKU, N'') NOT IN ('0240000000017', '0240000000031', 'duplicate', ''))  GROUP BY s.Supplier, p.Supplier_ID ORDER BY Supplier">
             </asp:SqlDataSource>

    <asp:DropDownList ID="DDL_isnew" runat="server" Visible="true">
        <asp:ListItem Value="1,0">New and Old</asp:ListItem>
         <asp:ListItem Value="1">Is new</asp:ListItem>
         <asp:ListItem Value="0">Not New</asp:ListItem>
     </asp:DropDownList>
      <asp:DropDownList ID="DDL_isdiscontinued" runat="server" Visible="true">
              <asp:ListItem Value="0">Active</asp:ListItem>
         <asp:ListItem Value="1">Discontinued</asp:ListItem>
         <asp:ListItem Value="1,0">Active & Discontinued</asp:ListItem>
     </asp:DropDownList>
      <asp:DropDownList ID="DDL_isprimary" runat="server" Visible="true">
         <asp:ListItem Value="1">Primary</asp:ListItem>
          <asp:ListItem Value="1,0">Primary & Secondry</asp:ListItem>
         <asp:ListItem Value="0">Secondry</asp:ListItem>
     </asp:DropDownList>
     <asp:DropDownList ID="DDl_ishalloween" runat="server" Visible="true">
          <asp:ListItem Value="1,0">Halloween and othetr</asp:ListItem>
         <asp:ListItem Value="1">is halloween</asp:ListItem>
         <asp:ListItem Value="0">Not halloween</asp:ListItem>
     </asp:DropDownList>
 <asp:DropDownList ID="DDL_SRP_Only" runat="server" Visible="true">
          <asp:ListItem Value="1,0">SRP Both</asp:ListItem>
         <asp:ListItem Value="1">SRP Only</asp:ListItem>
         <asp:ListItem Value="0">Not SRP</asp:ListItem>
     </asp:DropDownList>
     <asp:DropDownList ID="DDL_ACC" runat="server" Visible="true">
          <asp:ListItem Value="1,0">Acc&Cost</asp:ListItem>
         <asp:ListItem Value="1">Acc</asp:ListItem>
         <asp:ListItem Value="0">Cost</asp:ListItem>
     </asp:DropDownList>
	<asp:DropDownList ID="DDL_QtyOrValue" runat="server" Visible="true">
		<asp:ListItem Value="0">By Quatity</asp:ListItem>
        <asp:ListItem Value="1">By Value</asp:ListItem>
     </asp:DropDownList>
	 Display Price : 
	<asp:DropDownList ID="DDL_PriceFilter" runat="server" Visible="true">
		<asp:ListItem Value="All">ALL</asp:ListItem>
		<asp:ListItem Value="Equals">Equals to</asp:ListItem>
        <asp:ListItem Value="Greater">Greater Than</asp:ListItem>
		<asp:ListItem Value="Less">Less than</asp:ListItem>
	</asp:DropDownList>
	<asp:DropDownList ID="DDL_PriceRange" runat="server" Visible="true">
		<asp:ListItem Value="0">0</asp:ListItem>
		<asp:ListItem Value="1">1</asp:ListItem>
		<asp:ListItem Value="5">5</asp:ListItem>
        <asp:ListItem Value="10">10</asp:ListItem>
		<asp:ListItem Value="20">20</asp:ListItem>
		<asp:ListItem Value="30">30</asp:ListItem>
		<asp:ListItem Value="40">40</asp:ListItem>
		<asp:ListItem Value="50">50</asp:ListItem>
	</asp:DropDownList>
	<asp:Button ID="Button4" runat="server" Text="Configure" />&nbsp;
	<asp:Button ID="btnExportToExcel" runat="server" onclick="btnExportToExcel_Click" Text="Export To Excel" Visible="True" Width="215px" />
	<asp:Button ID="btnDisplayReport" runat="server" Text="Display Report"  Width="215px"  />			 
             <br>

            </div>

    <br />

    <asp:TextBox ID="txtContactsSearch" runat="server" OnTextChanged="txtContactsSearch_TextChanged" ViewStateMode="Disabled">
</asp:TextBox>
    <asp:Button ID="Button29" runat="server" Text="Select" />
<cc1:autocompleteextender ServiceMethod="SearchCustomers"
    MinimumPrefixLength="2"
    CompletionInterval="100" EnableCaching="false" CompletionSetCount="10"
    TargetControlID="txtContactsSearch"
    ID="AutoCompleteExtender1" runat="server" >
</cc1:autocompleteextender>
    <br />
<asp:UpdatePanel ID="UpdatePanel" runat="server" >
       <Triggers>
        <asp:AsyncPostBackTrigger ControlID="txtContactsSearch" />
    </Triggers>
        <ContentTemplate>  
     
        </ContentTemplate>
    </asp:UpdatePanel>


    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" DataSourceID="" Font-Size="X-Small" 
        Caption=" SALE AGGREGATED TO PRIMARY PRODUCT">
        <AlternatingRowStyle BackColor="#99CCFF" />
        <Columns>
            <asp:TemplateField HeaderText="#" >
                <ItemTemplate>
                    <asp:Label ID="Label2a" runat="server" Text='<%# Container.DataItemIndex + 1%>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
          
            <asp:BoundField DataField="Product_SKU" HeaderText="Product_SKU" SortExpression="Product_SKU" />
            
            <asp:BoundField DataField="Supplier" HeaderText="Supplier" ReadOnly="True" SortExpression="Supplier" />
                <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID">
                    <ItemTemplate>
                    <%# "<A HREF=""ProductPricingDetail.aspx?Product_type=SALE&Product_ID=" & Eval("Product_ID") & " ""  target='_blank'  >" & Eval("[Product_ID]") & "</A>"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="center"  Width="60px" />
                </asp:TemplateField>
			<asp:BoundField DataField="Variation_SKU" HeaderText="Variation SKU" ReadOnly="True" SortExpression="Variation_SKU"  />
            <asp:TemplateField HeaderText="EPOS_Code" SortExpression="EPOS_Code">
                    <ItemTemplate>
                    <%# "<A HREF=""http://www.escapade.co.uk/ADMIN2/Products_Edit.asp?Product_type=SALE&Product_ID=" & Eval("Product_ID") & """ target='_blank'  >" & Eval("[EPOS_Code]") & "</A>"%>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="left" Width="30"  />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Product_Name" SortExpression="Product_Name">
                        <ItemTemplate>
                        <%# "<A HREF=""http://www.escapade.co.uk" & Eval("Folder_path") & "/" & Eval("File_name") & ".asp "" target='_blank'  >" & Eval("[Product_Name]") & "</A>"%>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="left"  />
                </asp:TemplateField>
                <asp:BoundField DataField="Size" HeaderText="Size"  SortExpression="Size" >
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            <asp:CheckBoxField DataField="IsDiscontinued" HeaderText="is Disc" SortExpression="IsDiscontinued" />
            <asp:CheckBoxField DataField="IsHalloween" HeaderText="is Hall" SortExpression="IsHalloween" />
            <asp:CheckBoxField DataField="isnew" HeaderText="is New" SortExpression="isnew" />
            <asp:TemplateField HeaderText="Buy Price" SortExpression="Buy_Price">
                <EditItemTemplate>
                    <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Buy_Price") %>'></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Buy_Price", "{0:c}") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Web_Price" HeaderText="Web Price" ReadOnly="True" SortExpression="Web_Price"  DataFormatString="{0:c}" />
            <asp:BoundField DataField="Sale_Price" HeaderText="Sale Price" ReadOnly="True" SortExpression="Sale_Price"  DataFormatString="{0:c}" />
            <asp:BoundField DataField="tax_code" HeaderText="VAT Rate" 
                SortExpression="tax_code" >
            <ItemStyle HorizontalAlign="Center" />
            </asp:BoundField>
            <asp:TemplateField HeaderText="margin" SortExpression="margin">
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# MiscClass.RoundToNearest(Eval("margin"), 1.01) & "%" %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Category_Name" HeaderText="Category_Name" ReadOnly="True" SortExpression="Category_Name" />
            <asp:BoundField DataField="ItemsSoldYear" HeaderText="Total Sold" ReadOnly="True" SortExpression="ItemsSoldYear" DataFormatString="{0:c}" />
            <asp:BoundField DataField="Qty_Sold" HeaderText="Qty Sold" ReadOnly="True" SortExpression="Qty_Sold" />

            <asp:BoundField DataField="Y_Last_1" HeaderText="LY 1" ReadOnly="True" SortExpression="Y_Last_1" />
            <asp:BoundField DataField="Y_Last_2" HeaderText="LY 2" ReadOnly="True" SortExpression="Y_Last_2" />
            <asp:BoundField DataField="Y_Last_3" HeaderText="LY 3" ReadOnly="True" SortExpression="Y_Last_3" />
            <asp:BoundField DataField="Y_Last_4" HeaderText="LY 4" ReadOnly="True" SortExpression="Y_Last_4" />
            <asp:BoundField DataField="Y_Last_5" HeaderText="LY 5" ReadOnly="True" SortExpression="Y_Last_5" />
            <asp:BoundField DataField="Y_Last_6" HeaderText="LY 6" ReadOnly="True" SortExpression="Y_Last_6" />
            <asp:BoundField DataField="Y_Last_7" HeaderText="LY 7" ReadOnly="True" SortExpression="Y_Last_7" />
            <asp:BoundField DataField="Y_Last_8" HeaderText="LY 8" ReadOnly="True" SortExpression="Y_Last_8" />
            <asp:BoundField DataField="Y_Last_9" HeaderText="LY 9" ReadOnly="True" SortExpression="Y_Last_9" />
            <asp:BoundField DataField="Y_Last_10" HeaderText="LY 10" ReadOnly="True" SortExpression="Y_Last_10" />
            <asp:BoundField DataField="Y_Last_11" HeaderText="LY 11" ReadOnly="True" SortExpression="Y_Last_11" />
            <asp:BoundField DataField="Y_Last_12" HeaderText="LY 12" ReadOnly="True" SortExpression="Y_Last_12" />
            <asp:BoundField DataField="Y_Select_1" HeaderText="SY 1" ReadOnly="True" SortExpression="Y_Select_1" />
            <asp:BoundField DataField="Y_Select_2" HeaderText="SY 2" ReadOnly="True" SortExpression="Y_Select_2" />
            <asp:BoundField DataField="Y_Select_3" HeaderText="SY 3" ReadOnly="True" SortExpression="Y_Select_3" />
            <asp:BoundField DataField="Y_Select_4" HeaderText="SY 4" ReadOnly="True" SortExpression="Y_Select_4" />
            <asp:BoundField DataField="Y_Select_5" HeaderText="SY 5" ReadOnly="True" SortExpression="Y_Select_5" />
            <asp:BoundField DataField="Y_Select_6" HeaderText="SY 6" ReadOnly="True" SortExpression="Y_Select_6" />
            <asp:BoundField DataField="Y_Select_7" HeaderText="SY 7" ReadOnly="True" SortExpression="Y_Select_7" />
            <asp:BoundField DataField="Y_Select_8" HeaderText="SY 8" ReadOnly="True" SortExpression="Y_Select_8" />
            <asp:BoundField DataField="Y_Select_9" HeaderText="SY 9" ReadOnly="True" SortExpression="Y_Select_9" />
            <asp:TemplateField HeaderText="SY 10" SortExpression="Y_Select_10">
                <ItemTemplate>
                    <asp:Label ID="Y_Select_10" runat="server" Text='<%# eval("Y_Select_10") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Y_Select_11" HeaderText="SY 11" ReadOnly="True" SortExpression="Y_Select_11" />
            <asp:BoundField DataField="Y_Select_12" HeaderText="SY 12" ReadOnly="True" SortExpression="Y_Select_12" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="Widd_Qty" HeaderText="WH Qty" SortExpression="Widd_Qty" />
            <asp:BoundField DataField="Stock_Qty" HeaderText="Shop Qty" SortExpression="Stock_Qty" />
            <asp:BoundField DataField="tot_qty" HeaderText="Tot Qty" ReadOnly="True" SortExpression="tot_qty" />
            <asp:BoundField DataField="inPipeline" HeaderText="Pipe" SortExpression="inPipeline" />
			<asp:BoundField DataField="Qty_Sold_amz" HeaderText="Amazon Sale" SortExpression="Qty_Sold_amz" />
			
        <asp:BoundField DataField="viewsdays30" HeaderText="30 Day Views" SortExpression="viewsdays30" />
        <asp:BoundField DataField="scr" HeaderText="score" SortExpression="scr" />
			<asp:BoundField DataField="MarAug_Suggest" HeaderText="MAR AUG  Suggest" SortExpression="MarAug_Suggest" DataFormatString="{0:N0}" />
 <asp:BoundField DataField="SepDec_Suggest" HeaderText="SEP DEC  Suggest" SortExpression="SepDec_Suggest"  DataFormatString="{0:N0}" />
 <asp:BoundField DataField="janAug_Plus_Suggest" HeaderText="JAN AUG Suggest" SortExpression="janAug_Plus_Suggest"  DataFormatString="{0:N0}" /> 
	   <asp:BoundField DataField="Buy_3m" HeaderText="Buy 3m" SortExpression="Buy_3m" />
        <asp:BoundField DataField="Buy_6m" HeaderText="Buy 6m" SortExpression="Buy_6m" />   
         <asp:BoundField DataField="Requested" HeaderText="Req" ReadOnly="True" SortExpression="Requested" /> 
       <asp:BoundField DataField="SRP_only" HeaderText="SRP Only" SortExpression="SRP_only" />
            <asp:BoundField DataField="SaleType" HeaderText="Sale Type" SortExpression="SaleType" />
              <asp:BoundField DataField="returned" HeaderText="Ret" SortExpression="returned" />   
             <asp:BoundField DataField="Accessory" HeaderText="is Acc" SortExpression="Accessory" />   
         <asp:TemplateField HeaderText="Wish&lt;br&gt;Qty" SortExpression="Qty_Wish">

                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink4" runat="server"  NavigateUrl='<%#"PurchaseWishList.aspx?Product_type=SALE&Supplier_id=" & Eval("Supplier_ID")%>' Target="_blank">
                            <asp:Label ID="lbl_Qty_Wish" runat="server" Text='<%# Eval("Qty_Wish") & "/" & Eval("Qty_Wish_hal")%>'></asp:Label>
                        </asp:HyperLink>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>      
         <asp:TemplateField HeaderText="Add<br>Wish" InsertVisible="False" SortExpression="convertedPrice" Visible="true">
            <ItemTemplate>
                <asp:Button ID="btnTypeSizeCommodity1" runat="server" Text="Add"  
                CommandName="MakeWish" 
                CommandArgument ='<%# Eval("Product_ID") & "^" & Eval("widd_qty") & "^" & Eval("amazon_qty") & "^" & Eval("Stock_Qty") & "^" & Container.DataItemIndex%>'
                Font-Size="XX-Small" BackColor="lightgreen"  ForeColor="black"  UseSubmitBehavior="false"  /> 
            </ItemTemplate>
        </asp:TemplateField>

        </Columns>
    </asp:GridView>
	    	
<asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
        
        
        SelectCommand="SELECT TOP (CASE @TOP500 WHEN 1 THEN 500 ELSE 9999999 END) Product_SKU,Variation_SKU,  EPOS_Code, Product_ID,  IsDiscontinued, SRP_only, SaleType,Supplier_ID, Supplier, Size, Product_Name, returned, requested 
		, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0)- isnull(amazon_qty, 0)) ELSE 0 END ELSE 0 END) ELSE '-' END AS Buy_3m 
		, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) ELSE 0 END ELSE 0 END) ELSE '-' END AS Buy_6m , CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0)- isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &lt; 0 THEN - 1 * ((((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0))) ELSE 0 END ELSE 0 END) ELSE '-' END AS Surplus_6m ,CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold)* 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &lt; 0 THEN - 1 * ((((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0))) ELSE 0 END ELSE 0 END) ELSE '-' END AS Surplus_3m, CASE WHEN monthssold &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) ELSE 0 END AS sixmonthavg, CASE WHEN monthssold &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) ELSE 0 END AS threemonthavg , WH_Qty AS WH_Qty , isnull(Qty_Sold, 0) Qty_Sold , whichOctobervalue , OCt80pc, OCt25pc, monthssold, scr  , viewsdays30, Web_Price, Sale_Price, Price_Offered, isnull(Buy_Price, 0) Buy_Price, ISNULL(Margin, 0) AS Margin, Category_ID, Category_Name, ItemsSoldYear
		, Qty_Sold AS Qty_Sold
		,isnull(Qty_Sold_amz,0)Qty_Sold_amz
		, Y_last_1, Y_last_2, Y_last_3, Y_last_4, Y_last_5, Y_last_6, Y_last_7, Y_last_8, Y_last_9, Y_last_10, Y_last_11, Y_last_12, Y_Select_1, Y_Select_2, Y_Select_3, Y_Select_4, Y_Select_5, Y_Select_6, Y_Select_7, Y_Select_8, Y_Select_9, Y_Select_10, Y_Select_11, Y_Select_12, Widd_Qty, Stock_Qty, tot_qty, In_Transit, inPipeline
		, JanFeb_SY AS JanFeb_SY,MarAug_SY AS MarAug_SY, SepDec_sy AS SepDec_sy, janAug_SYPlus AS janAug_SYPlus ,MarAug_Suggest,SepDec_Suggest,janAug_Plus_Suggest
		, Yearly_Sale, isPrimary, Product_Type, IsHalloween, IsNew, Tax_code, amazon_qty, ebay_qty, Accessory, Qty_Wish, Qty_Wish_hal, Folder_path, File_name, ROW_NUMBER() OVER (ORDER BY isnull(Qty_Sold, 0) DESC) 
		
		FROM (SELECT ISNULL(Y_Select_10, Y_last_10) AS whichOctobervalue, CASE WHEN Qty_Sold - isnull(Y_Select_10, Y_last_10) &gt; Qty_Sold * .8 THEN 1 ELSE 0 END AS OCt80pc, CASE WHEN Qty_Sold - isnull(Y_Select_10, Y_last_10) &gt; Qty_Sold * .25 THEN 1 ELSE 0 END AS OCt25pc, (CASE WHEN isnull(Y_Select_1, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_2, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_3, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_4, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_5, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_6, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_7, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_8, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_9, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_10, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_11, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_12, 0) &gt; 0 THEN 1 ELSE 0 END) AS monthssold, scr, viewsdays30, Supplier, Supplier_ID, Product_ID,Variation_SKU, Size, EPOS_Code, Product_Name, Web_Price, Sale_Price, Price_Offered, Buy_Price, Margin, Category_ID, Category_Name, ItemsSoldYear, Qty_Sold, Y_last_1, Y_last_2, Y_last_3, Y_last_4, Y_last_5, Y_last_6, Y_last_7, Y_last_8, Y_last_9, Y_last_10, Y_last_11, Y_last_12, Y_Select_1, Y_Select_2, Y_Select_3, Y_Select_4, Y_Select_5, Y_Select_6, Y_Select_7, Y_Select_8, Y_Select_9, Y_Select_10, Y_Select_11, Y_Select_12, Widd_Qty, Sup_ID, IsDiscontinued, SRP_only, SaleType, Stock_Qty, tot_qty, In_Transit, inPipeline, Yearly_Sale, Product_SKU, isPrimary, Folder_path, File_name, Product_Type, IsHalloween, IsNew, Tax_code, amazon_qty, ebay_qty, Accessory, isnull(Qty_Wish, 0) Qty_Wish, isnull(Qty_Wish_hal, 0) Qty_Wish_hal, requested, returned, qttyyy, JanFeb_SY,MarAug_SY, SepDec_SY, janAug_SYPlus, WH_Qty 
		,Qty_Sold_amz
		,CASE WHEN JanFeb_SY &lt; WH_Qty THEN 0 ELSE MarAug_SY - (CASE WHEN (WH_Qty-JanFeb_SY)&gt; 0 THEN (WH_Qty-JanFeb_SY)ELSE 0 END) END AS MarAug_Suggest 
		,CASE WHEN SepDec_SY &lt; (WH_Qty-janAug_SYPlus) THEN 0 ELSE (SepDec_SY - (CASE WHEN (WH_Qty-janAug_SYPlus)&gt; 0 THEN (WH_Qty-janAug_SYPlus) ELSE 0 END) )END AS SepDec_Suggest 
		,CASE WHEN janAug_SYPlus &lt; (WH_Qty-(janAug_SYPlus+SepDec_SY)) THEN 0 ELSE janAug_SYPlus -(CASE WHEN (WH_Qty-(janAug_SYPlus+SepDec_SY)) &gt; 0 THEN (WH_Qty-(janAug_SYPlus+SepDec_SY)) ELSE 0 END) END AS janAug_Plus_Suggest 
		
		FROM (SELECT TOP (100) PERCENT ROUND(ISNULL(ps3.scr, 0), 3) AS scr, vv.viewsdays30, ISNULL(Suppliers.Supplier, 'N/A') AS Supplier, Products.Supplier_ID, Products.Product_ID,Products.Variation_SKU , max(Products.Widd_Qty + ISNULL(Products.ebay_qty, 0) + ISNULL(Products.amazon_qty, 0))AS WH_Qty, Products.Product_Size AS Size, Products.EPOS_Code, Products.Product_Name, ROUND(Products.Web_Price, 2) AS Web_Price, ROUND(Products.Sale_Price, 2) AS Sale_Price, ISNULL(Products.Sale_Price, Products.Web_Price) AS Price_Offered, Products.Buy_Price 
		/*-- Margin Calculation --*/
		, (CASE isnull(products.Tax_code, 0)WHEN 0 THEN ((ISNULL(Products.Sale_Price, Products.Web_Price) - (Products.Buy_Price + (CASE WHEN products.saletype = 'Clearance' AND SALE_PRICE &gt; 0  THEN 2.5 ELSE 0 END))) * 100 / ISNULL(Products.Sale_Price, Products.Web_Price)) ELSE ((((ISNULL(Products.Sale_Price, Products.Web_Price) * 100 / (100 + products.Tax_code)) - (Products.Buy_Price + (CASE WHEN products.saletype = 'Clearance' AND SALE_PRICE &gt; 0  THEN 2.5 ELSE 0 END)))) * 100) / ((ISNULL(Products.Sale_Price, Products.Web_Price) * 100) / (100 + products.Tax_code)) END) AS Margin 
		/*-- eo Margin Calculation --*/
		
		, ISNULL(Products.Category_ID, 0) AS Category_ID, ISNULL(Categories.Category_Name, N'') AS Category_Name
		, SUM(CASE WHEN 1 = 1 THEN isnull(SalesOrder_Items.Quantity * SalesOrder_Items.Price, 0) ELSE 0 END) AS ItemsSoldYear
		, SUM(CASE WHEN 1 = 1 THEN SalesOrder_Items.Quantity ELSE 0 END) AS Qty_Sold
          
		, SUM(CASE WHEN 1 = 1 and SalesOrder_Items.Paymentvia = 'Amazon' THEN SalesOrder_Items.Quantity  ELSE 0 END) AS Qty_Sold_amz 
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 1 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_1
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 2 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_2
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 3 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_3
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 4 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_4
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 5 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_5
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 6 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_6
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 7 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_7
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 8 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_8
		, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 9 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_9, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 10 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_10, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 11 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_11, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 12 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_12, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 1 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_1, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 2 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_2, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 3 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_3, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 4 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_4, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 5 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_5, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 6 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_6, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 7 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_7, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 8 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_8, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 9 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_9, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 10 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_10, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 11 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_11, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 12 THEN (CASE @val WHEN 1 THEN SalesOrder_Items.amount ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_12, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.amazon_qty, 0) + ISNULL(Products.ebay_qty, 0) AS Widd_Qty, Suppliers.Sup_ID, Products.IsDiscontinued, Products.SRP_only, SaleType, Products.Stock_Qty, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.Stock_Qty, 0) AS tot_qty, Products.In_Transit, Vw_PurchasePipeline.inPipeline, SUM(ISNULL(ISNULL(Products.Sale_Price, Products.Web_Price) * ISNULL(SalesOrder_Items.Quantity, 0), 0)) AS Yearly_Sale, Products.Product_SKU, Products.isPrimary, Products.Folder_path, Products.File_name, Products.Product_Type, Products.IsHalloween, Products.IsNew, Products.Tax_code, ISNULL(Products.amazon_qty, 0) AS amazon_qty, ISNULL(Products.ebay_qty, 0) AS ebay_qty, Products.[Accessory], pwl.Qty_Wish, pwl.Qty_Wish_hal, requested.cnt AS requested, GoodReturned.returned, SUM(SalesOrder_Items.Quantity) AS qttyyy , (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 1 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 2) THEN ( SalesOrder_Items.Quantity ) ELSE 0 END))AS JanFeb_SY , (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 3 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 8) THEN ( SalesOrder_Items.Quantity ) ELSE 0 END))AS MarAug_SY , (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 9 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 12) THEN ( SalesOrder_Items.Quantity ) ELSE 0 END) )AS SepDec_SY , SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) &lt;= 8 THEN (SalesOrder_Items.Quantity) ELSE 0 END) AS janAug_SYPlus 
		FROM Products WITH (NOLOCK) 
		LEFT OUTER JOIN Categories WITH (NOLOCK) ON Categories.Category_ID = Products.Category_ID 
		LEFT OUTER JOIN Suppliers WITH (NOLOCK) ON Suppliers.Sup_ID = Products.Supplier_ID 
		LEFT OUTER JOIN Vw_PurchasePipeline WITH (NOLOCK) ON Products.Product_SKU = Vw_PurchasePipeline.SKU 
		LEFT OUTER JOIN BI_ViewsSummary AS vv ON vv.Product_id = Products.Product_ID 
		LEFT OUTER JOIN tbl_Ptoduct_Score5 AS ps3 ON ps3.Product_id = Products.Product_ID 
		LEFT OUTER JOIN (SELECT Product_id, SUM(ISNULL(ISNULL(Qty_Wished_Replanish, Qty_recomended_Replanish), 0)) AS Qty_Wish, SUM(ISNULL(ISNULL(Qty_Wished_Halloween, Qty_Recomended_Halloween), 0)) AS Qty_Wish_hal 
		FROM PurchaseWishList WITH (NOLOCK) WHERE (ISNULL(Status, N'') &lt;&gt; 'closed') GROUP BY Product_id) AS pwl ON pwl.Product_id = Products.Product_ID 
		LEFT OUTER JOIN (SELECT Product_ID, COUNT(*) AS cnt FROM Customers_Instock_Alert AS r WHERE (YEAR(TimeStamp) = @YEARFROM) AND (MONTH(TimeStamp) &lt;= @MONTH) GROUP BY Product_ID) AS requested ON requested.Product_ID = Products.Product_ID 
		LEFT OUTER JOIN (SELECT sor.Product_ID, SUM(sor.Quantity) AS returned FROM SalesOrder_Returns AS sor LEFT OUTER JOIN SalesOrder AS so ON so.Order_ID = sor.Order_ID GROUP BY sor.Product_ID) AS GoodReturned ON GoodReturned.Product_ID = Products.Product_ID 
		LEFT OUTER JOIN (SELECT soi.*, so.orderdate, so.Paymentvia FROM SalesOrder_Items soi WITH (NOLOCK) LEFT JOIN SalesOrder so WITH (NOLOCK) ON so.Order_ID = soi.Order_ID WHERE (1 = 1) AND (so.OrderDate BETWEEN dateadd(year, - 2, dbo.FN_EOMONTH('01/' + CONVERT(char(3), DATEADD(month, @month - 1, 0), 0) + '/' + CONVERT(nvarchar(6), @YEARFROM))) AND dbo.FN_EOMONTH('01/' + CONVERT(char(3), DATEADD(month, @month - 1, 0), 0) + '/' + CONVERT(nvarchar(6), @YEARFROM))) 
		AND so.PaymentConfirm = 1 AND (1 = (CASE WHEN @country = 'All' THEN 1 WHEN so.ShippingCountry = @country THEN 1 ELSE 0 END))) AS SalesOrder_Items ON Products.Product_ID = SalesOrder_Items.Product_ID 
		WHERE 1 = 1 
		AND (1 = CASE WHEN @Product_SKU = '0' THEN 1 WHEN Products.Product_SKU = @Product_SKU THEN 1 ELSE 0 END) 
		AND (1 = CASE WHEN Products.Product_id = @product_id THEN 1 WHEN 0 = @product_id THEN 1 ELSE 0 END) 
		AND (1 = CASE WHEN @product_id = 0 THEN 1 WHEN Products.Product_id = @product_id THEN 1 ELSE 0 END) 
		AND (1 = (CASE @category WHEN 0 THEN 1 WHEN Products.Category_ID THEN 1 ELSE 0 END)) 
		AND (1 = (CASE @Supplier WHEN 0 THEN 1 WHEN Products.Supplier_ID THEN 1 ELSE 0 END)) 
		AND (1 = (CASE WHEN @ishalloween = '1,0' THEN 1 ELSE CASE WHEN CONVERT(bit, @ishalloween) = products.ishalloween THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @isnew = '1,0' THEN 1 ELSE CASE WHEN isnew = CONVERT(bit, @isNew) THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @isDiscontinued = '1,0' THEN 1 ELSE CASE WHEN CONVERT(bit, @isDiscontinued) = '0' AND (isDiscontinued = @isDiscontinued OR ((isnull(isDiscontinued, 0) = 1 AND Widd_Qty &gt; 0) OR (isnull(isDiscontinued, 0) = 1 AND AnySize_inStock = 1))) THEN 1 WHEN CONVERT(bit, @isDiscontinued) = isDiscontinued THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @Gender = '0' THEN 1 ELSE CASE WHEN Gender = (@Gender) THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @CA = '999' THEN 1 ELSE CASE WHEN tax_code = CONVERT(int, @CA) THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @Accessory = '1,0' THEN 1 ELSE CASE WHEN Accessory = CONVERT(bit, @Accessory) THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @isPrimary = '1,0' THEN 1 ELSE CASE WHEN CONVERT(bit, @isPrimary) = isPrimary THEN 1 ELSE 0 END END)) 
		AND (1 = (CASE WHEN @SRP_only = '1,0' THEN 1 ELSE CASE WHEN CONVERT(int, @SRP_only) = CONVERT(int, SRP_only) THEN 1 ELSE 0 END END)) 
		AND (ISNULL(Products.Product_SKU, N'') NOT IN ('Duplicate', '0240000000017', '0240000000031')) 
		AND (Products.Tax_code &lt;&gt; '3.5') 
		AND isnull(Web_Price, 0) &lt;&gt; 0 AND Product_Type = 'sale' 
		AND (1 = (CASE WHEN @PriceFilter = 'All' THEN 1 ELSE 
		CASE 
		WHEN @PriceFilter = 'greater' and isnull(SAle_Price,web_price) >= @PriceRange THEN 1 
		WHEN @PriceFilter = 'less' and isnull(SAle_Price,web_price) <= @PriceRange THEN 1 
		WHEN @PriceFilter = 'equal' and isnull(SAle_Price,web_price) = @PriceRange THEN 1 
		ELSE 0 END END))
		GROUP BY Products.IsNew, Products.IsHalloween, vv.viewsdays30, Products.Product_ID,Products.Variation_SKU, Products.Product_Size
		, Products.EPOS_Code, Products.Product_Name, Products.Category_ID, ISNULL(Categories.Category_Name, N''), Products.Web_Price
		, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.amazon_qty, 0) + ISNULL(Products.ebay_qty, 0), Suppliers.Supplier, Products.Supplier_ID
		, Suppliers.Sup_ID, Products.IsDiscontinued, Products.SRP_only, Products.SaleType, Products.Stock_Qty, Products.Buy_Price
		, Products.In_Transit, Vw_PurchasePipeline.inPipeline, Products.Sale_Price, Products.Product_SKU, Products.isPrimary, Products.Folder_path
		, Products.File_name, ISNULL(Categories.Category_Name, N'NA'), Products.Product_Type, Products.AnySize_inStock, Products.InStock
		, Products.Widd_Qty, ps3.scr, Products.Tax_code, Products.amazon_qty, Products.ebay_qty, pwl.Qty_Wish, pwl.Qty_Wish_hal, requested.cnt
		, GoodReturned.returned, Products.[Accessory]) AS ddd) AS ff">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="1" Name="TOP500"  QueryStringField="top500" />
            <asp:QueryStringParameter DefaultValue="2016" Name="YEARFROM"  QueryStringField="yearfrom" />
			<asp:QueryStringParameter DbType="String" DefaultValue="0"  Name="val" QueryStringField="val" />
            <asp:QueryStringParameter DefaultValue="1" Name="MONTH"  QueryStringField="mmonth" />
			 <asp:QueryStringParameter  DefaultValue="ALL" Name="country"   QueryStringField="mcountry"  />
            <asp:QueryStringParameter DefaultValue="0" Name="Product_SKU" QueryStringField="sku" />
            <asp:QueryStringParameter DefaultValue="0" Name="product_id" QueryStringField="Product_id" />
            <asp:QueryStringParameter DefaultValue="0" Name="category"     QueryStringField="Catagories" />
            <asp:QueryStringParameter DefaultValue="0" Name="Supplier"     QueryStringField="Supplier" />
            <asp:QueryStringParameter DbType="String" DefaultValue="1,0" Name="ishalloween"   QueryStringField="ishalloween" />
            <asp:QueryStringParameter DbType="String" DefaultValue="1,0" Name="isNew"  QueryStringField="isnew" />
            <asp:QueryStringParameter DbType="String" DefaultValue="0"  Name="isDiscontinued" QueryStringField="isdiscontinued" />
            <asp:QueryStringParameter DefaultValue="ALL" Name="Gender" QueryStringField="Gender" />
            <asp:QueryStringParameter DefaultValue="999" Name="CA" QueryStringField="CA" />
             <asp:QueryStringParameter DbType="String" DefaultValue="1,0"  Name="Accessory" QueryStringField="Acc" />
            <asp:QueryStringParameter   DefaultValue="1" Name="isPrimary" QueryStringField="isprimary" />
            <asp:QueryStringParameter DbType="String" DefaultValue="1,0"  Name="SRP_only" QueryStringField="SRP_only" />
			<asp:QueryStringParameter DbType="String" DefaultValue="All"  Name="PriceRange" QueryStringField="PriceRange" />
			<asp:QueryStringParameter DbType="String" DefaultValue="All"  Name="PriceFilter" QueryStringField="PriceFilter" />
        </SelectParameters>
    </asp:SqlDataSource>
	
	
    <br />
    <asp:SqlDataSource ID="SqlDataSource_search" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Escapade_NewConnectionString %>" 
    
    SelectCommand="SELECT TOP (CASE @TOP500 WHEN 1 THEN 500 ELSE 9999999 END) Product_SKU, EPOS_Code, Product_ID,Variation_SKU, IsDiscontinued, SRP_only, SaleType, Supplier_ID, Supplier, Size, Product_Name, returned, requested, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) ELSE 0 END ELSE 0 END) ELSE '-' END AS Buy_3m, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) ELSE 0 END ELSE 0 END) ELSE '-' END AS Buy_6m, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &lt; 0 THEN - 1 * ((((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0))) ELSE 0 END ELSE 0 END) ELSE '-' END AS Surplus_6m, CASE WHEN monthssold &gt; 0 THEN (CASE WHEN (Qty_Sold) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &gt;= 0 AND OCt80pc = 0 THEN CASE WHEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0)) &lt; 0 THEN - 1 * ((((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) + (- WIDD_QTY - isnull(ebay_qty, 0) - isnull(amazon_qty, 0))) ELSE 0 END ELSE 0 END) ELSE '-' END AS Surplus_3m, CASE WHEN monthssold &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 6) ELSE 0 END AS sixmonthavg, CASE WHEN monthssold &gt; 0 THEN (((Qty_Sold) - whichOctobervalue) / (monthssold) * 3) ELSE 0 END AS threemonthavg, WH_Qty AS WH_Qty, isnull(Qty_Sold, 0) Qty_Sold, whichOctobervalue, OCt80pc, OCt25pc, monthssold, scr, viewsdays30, Web_Price, Sale_Price, Price_Offered, isnull(Buy_Price, 0) Buy_Price, ISNULL(Margin, 0) AS Margin, Category_ID, Category_Name, ItemsSoldYear, Qty_Sold AS Qty_Sold, isnull(Qty_Sold_amz, 0) Qty_Sold_amz, Y_last_1, Y_last_2, Y_last_3, Y_last_4, Y_last_5, Y_last_6, Y_last_7, Y_last_8, Y_last_9, Y_last_10, Y_last_11, Y_last_12, Y_Select_1, Y_Select_2, Y_Select_3, Y_Select_4, Y_Select_5, Y_Select_6, Y_Select_7, Y_Select_8, Y_Select_9, Y_Select_10, Y_Select_11, Y_Select_12, Widd_Qty, Stock_Qty, tot_qty, In_Transit, inPipeline, JanFeb_SY AS JanFeb_SY, MarAug_SY AS MarAug_SY, SepDec_sy AS SepDec_sy, janAug_SYPlus AS janAug_SYPlus, MarAug_Suggest, SepDec_Suggest, janAug_Plus_Suggest, Yearly_Sale, isPrimary, Product_Type, IsHalloween, IsNew, Tax_code, amazon_qty, ebay_qty, Accessory, Qty_Wish, Qty_Wish_hal, Folder_path, File_name, ROW_NUMBER() OVER (ORDER BY isnull(Qty_Sold, 0) DESC) FROM (SELECT ISNULL(Y_Select_10, Y_last_10) AS whichOctobervalue, CASE WHEN Qty_Sold - isnull(Y_Select_10, Y_last_10) &gt; Qty_Sold * .8 THEN 1 ELSE 0 END AS OCt80pc, CASE WHEN Qty_Sold - isnull(Y_Select_10, Y_last_10) &gt; Qty_Sold * .25 THEN 1 ELSE 0 END AS OCt25pc, (CASE WHEN isnull(Y_Select_1, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_2, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_3, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_4, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_5, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_6, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_7, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_8, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_9, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_10, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_11, 0) &gt; 0 THEN 1 ELSE 0 END) + (CASE WHEN isnull(Y_Select_12, 0) &gt; 0 THEN 1 ELSE 0 END) AS monthssold, scr, viewsdays30, Supplier, Supplier_ID, Product_ID,Variation_SKU, Size, EPOS_Code, Product_Name, Web_Price, Sale_Price, Price_Offered, Buy_Price, Margin, Category_ID, Category_Name, ItemsSoldYear, Qty_Sold, Y_last_1, Y_last_2, Y_last_3, Y_last_4, Y_last_5, Y_last_6, Y_last_7, Y_last_8, Y_last_9, Y_last_10, Y_last_11, Y_last_12, Y_Select_1, Y_Select_2, Y_Select_3, Y_Select_4, Y_Select_5, Y_Select_6, Y_Select_7, Y_Select_8, Y_Select_9, Y_Select_10, Y_Select_11, Y_Select_12, Widd_Qty, Sup_ID, IsDiscontinued, SRP_only, SaleType, Stock_Qty, tot_qty, In_Transit, inPipeline, Yearly_Sale, Product_SKU, isPrimary, Folder_path, File_name, Product_Type, IsHalloween, IsNew, Tax_code, amazon_qty, ebay_qty, Accessory, isnull(Qty_Wish, 0) Qty_Wish, isnull(Qty_Wish_hal, 0) Qty_Wish_hal, requested, returned, qttyyy, JanFeb_SY, MarAug_SY, SepDec_SY, janAug_SYPlus, WH_Qty, Qty_Sold_amz, CASE WHEN JanFeb_SY &lt; WH_Qty THEN 0 ELSE MarAug_SY - (CASE WHEN (WH_Qty - JanFeb_SY) &gt; 0 THEN (WH_Qty - JanFeb_SY) ELSE 0 END) END AS MarAug_Suggest, CASE WHEN SepDec_SY &lt; (WH_Qty - janAug_SYPlus) THEN 0 ELSE (SepDec_SY - (CASE WHEN (WH_Qty - janAug_SYPlus) &gt; 0 THEN (WH_Qty - janAug_SYPlus) ELSE 0 END)) END AS SepDec_Suggest, CASE WHEN janAug_SYPlus &lt; (WH_Qty - (janAug_SYPlus + SepDec_SY)) THEN 0 ELSE janAug_SYPlus - (CASE WHEN (WH_Qty - (janAug_SYPlus + SepDec_SY)) &gt; 0 THEN (WH_Qty - (janAug_SYPlus + SepDec_SY)) ELSE 0 END) END AS janAug_Plus_Suggest FROM (SELECT TOP (100) PERCENT ROUND(ISNULL(ps3.scr, 0), 3) AS scr, vv.viewsdays30, ISNULL(Suppliers.Supplier, 'N/A') AS Supplier, Products.Supplier_ID, Products.Product_ID,Products.Variation_SKU, max(Products.Widd_Qty + ISNULL(Products.ebay_qty, 0) + ISNULL(Products.amazon_qty, 0)) AS WH_Qty, Products.Product_Size AS Size, Products.EPOS_Code, Products.Product_Name, ROUND(Products.Web_Price, 2) AS Web_Price, ROUND(Products.Sale_Price, 2) AS Sale_Price, ISNULL(Products.Sale_Price, Products.Web_Price) AS Price_Offered, Products.Buy_Price , (CASE isnull(products.Tax_code, 0) WHEN 0 THEN ((ISNULL(Products.Sale_Price, Products.Web_Price) - (Products.Buy_Price + (CASE WHEN products.saletype = 'Clearance' AND SALE_PRICE &gt; 0 THEN 2.5 ELSE 0 END))) * 100 / ISNULL(Products.Sale_Price, Products.Web_Price)) ELSE ((((ISNULL(Products.Sale_Price, Products.Web_Price) * 100 / (100 + products.Tax_code)) - (Products.Buy_Price + (CASE WHEN products.saletype = 'Clearance' AND SALE_PRICE &gt; 0 THEN 2.5 ELSE 0 END)))) * 100) / ((ISNULL(Products.Sale_Price, Products.Web_Price) * 100) / (100 + products.Tax_code)) END) AS Margin , ISNULL(Products.Category_ID, 0) AS Category_ID, ISNULL(Categories.Category_Name, N'') AS Category_Name, SUM(CASE WHEN 1 = 1 THEN isnull(SalesOrder_Items.Quantity * SalesOrder_Items.Price, 0) ELSE 0 END) AS ItemsSoldYear, SUM(CASE WHEN 1 = 1 THEN SalesOrder_Items.Quantity ELSE 0 END) AS Qty_Sold, SUM(CASE WHEN 1 = 1 AND SalesOrder_Items.Paymentvia = 'Amazon' THEN SalesOrder_Items.Quantity ELSE 0 END) AS Qty_Sold_amz, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 1 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_1, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 2 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_2, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 3 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_3, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 4 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_4, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 5 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_5, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 6 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_6, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 7 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_7, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 8 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_8, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 9 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_9, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 10 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_10, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 11 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_11, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM - 1 AND MONTH(SalesOrder_Items.OrderDate) = 12 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_last_12, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 1 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_1, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 2 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_2, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 3 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_3, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 4 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_4, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 5 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_5, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 6 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_6, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 7 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_7, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 8 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_8, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 9 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_9, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 10 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_10, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 11 THEN (CASE @val WHEN 1 THEN CONVERT(int, SalesOrder_Items.amount) ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_11, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) = 12 THEN (CASE @val WHEN 1 THEN SalesOrder_Items.amount ELSE SalesOrder_Items.Quantity END) ELSE 0 END) AS Y_Select_12, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.amazon_qty, 0) + ISNULL(Products.ebay_qty, 0) AS Widd_Qty, Suppliers.Sup_ID, Products.IsDiscontinued, Products.SRP_only, SaleType, Products.Stock_Qty, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.Stock_Qty, 0) AS tot_qty, Products.In_Transit, Vw_PurchasePipeline.inPipeline, SUM(ISNULL(ISNULL(Products.Sale_Price, Products.Web_Price) * ISNULL(SalesOrder_Items.Quantity, 0), 0)) AS Yearly_Sale, Products.Product_SKU, Products.isPrimary, Products.Folder_path, Products.File_name, Products.Product_Type, Products.IsHalloween, Products.IsNew, Products.Tax_code, ISNULL(Products.amazon_qty, 0) AS amazon_qty, ISNULL(Products.ebay_qty, 0) AS ebay_qty, Products.[Accessory], pwl.Qty_Wish, pwl.Qty_Wish_hal, requested.cnt AS requested, GoodReturned.returned, SUM(SalesOrder_Items.Quantity) AS qttyyy, (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 1 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 2) THEN (SalesOrder_Items.Quantity) ELSE 0 END)) AS JanFeb_SY, (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 3 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 8) THEN (SalesOrder_Items.Quantity) ELSE 0 END)) AS MarAug_SY, (SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND (MONTH(SalesOrder_Items.OrderDate) &gt;= 9 AND MONTH(SalesOrder_Items.OrderDate) &lt;= 12) THEN (SalesOrder_Items.Quantity) ELSE 0 END)) AS SepDec_SY, SUM(CASE WHEN year(SalesOrder_Items.OrderDate) = @YEARFROM AND MONTH(SalesOrder_Items.OrderDate) &lt;= 8 THEN (SalesOrder_Items.Quantity) ELSE 0 END) AS janAug_SYPlus FROM Products WITH (NOLOCK) LEFT OUTER JOIN Categories WITH (NOLOCK) ON Categories.Category_ID = Products.Category_ID LEFT OUTER JOIN Suppliers WITH (NOLOCK) ON Suppliers.Sup_ID = Products.Supplier_ID LEFT OUTER JOIN Vw_PurchasePipeline WITH (NOLOCK) ON Products.Product_SKU = Vw_PurchasePipeline.SKU LEFT OUTER JOIN BI_ViewsSummary AS vv ON vv.Product_id = Products.Product_ID LEFT OUTER JOIN tbl_Ptoduct_Score5 AS ps3 ON ps3.Product_id = Products.Product_ID LEFT OUTER JOIN (SELECT Product_id, SUM(ISNULL(ISNULL(Qty_Wished_Replanish, Qty_recomended_Replanish), 0)) AS Qty_Wish, SUM(ISNULL(ISNULL(Qty_Wished_Halloween, Qty_Recomended_Halloween), 0)) AS Qty_Wish_hal FROM PurchaseWishList WITH (NOLOCK) WHERE (ISNULL(Status, N'') &lt;&gt; 'closed') GROUP BY Product_id) AS pwl ON pwl.Product_id = Products.Product_ID LEFT OUTER JOIN (SELECT Product_ID, COUNT(*) AS cnt FROM Customers_Instock_Alert AS r WHERE (YEAR(TimeStamp) = @YEARFROM) AND (MONTH(TimeStamp) &lt;= @MONTH) GROUP BY Product_ID) AS requested ON requested.Product_ID = Products.Product_ID LEFT OUTER JOIN (SELECT sor.Product_ID, SUM(sor.Quantity) AS returned FROM SalesOrder_Returns AS sor LEFT OUTER JOIN SalesOrder AS so ON so.Order_ID = sor.Order_ID GROUP BY sor.Product_ID) AS GoodReturned ON GoodReturned.Product_ID = Products.Product_ID LEFT OUTER JOIN (SELECT soi.*, so.orderdate, so.Paymentvia FROM SalesOrder_Items soi WITH (NOLOCK) LEFT JOIN SalesOrder so WITH (NOLOCK) ON so.Order_ID = soi.Order_ID WHERE (1 = 1) AND (so.OrderDate BETWEEN dateadd(year, - 2, dbo.FN_EOMONTH('01/' + CONVERT(char(3), DATEADD(month, @month - 1, 0), 0) + '/' + CONVERT(nvarchar(6), @YEARFROM))) AND dbo.FN_EOMONTH('01/' + CONVERT(char(3), DATEADD(month, @month - 1, 0), 0) + '/' + CONVERT(nvarchar(6), @YEARFROM))) AND so.PaymentConfirm = 1 AND (1 = (CASE WHEN @country = 'All' THEN 1 WHEN so.ShippingCountry = @country THEN 1 ELSE 0 END))) AS SalesOrder_Items ON Products.Product_ID = SalesOrder_Items.Product_ID 
        WHERE (Products.Product_Name LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') AND (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 0) OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 0) AND (Products.EPOS_Code LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 0) AND (Suppliers.Supplier LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 0) AND (Categories.Category_Name LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') OR (Products.Product_Name LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') AND (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 1) AND (Products.AnySize_inStock = 1) OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 1) AND (Products.EPOS_Code LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') AND (Products.AnySize_inStock = 1) OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 1) AND (Suppliers.Supplier LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') AND (Products.AnySize_inStock = 1) OR (ISNULL(Products.Hidden, 0) = 0) AND (Products.IsDiscontinued = 1) AND (Categories.Category_Name LIKE '%' + REPLACE(REPLACE(@SearchText, '  ', ' '), ' ', '%') + '%') AND (Products.AnySize_inStock = 1) GROUP BY Products.IsNew, Products.IsHalloween, vv.viewsdays30, Products.Product_ID,Products.Variation_SKU, Products.Product_Size, Products.EPOS_Code, Products.Product_Name, Products.Category_ID, ISNULL(Categories.Category_Name, N''), Products.Web_Price, ISNULL(Products.Widd_Qty, 0) + ISNULL(Products.amazon_qty, 0) + ISNULL(Products.ebay_qty, 0), Suppliers.Supplier, Products.Supplier_ID, Suppliers.Sup_ID, Products.IsDiscontinued, Products.SRP_only, Products.SaleType, Products.Stock_Qty, Products.Buy_Price, Products.In_Transit, Vw_PurchasePipeline.inPipeline, Products.Sale_Price, Products.Product_SKU, Products.isPrimary, Products.Folder_path, Products.File_name, ISNULL(Categories.Category_Name, N'NA'), Products.Product_Type, Products.AnySize_inStock, Products.InStock, Products.Widd_Qty, ps3.scr, Products.Tax_code, Products.amazon_qty, Products.ebay_qty, pwl.Qty_Wish, pwl.Qty_Wish_hal, requested.cnt, GoodReturned.returned, Products.[Accessory]) AS ddd) AS ff">
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="0" Name="TOP500"  QueryStringField="top500" />
            <asp:QueryStringParameter DefaultValue="2015" Name="YEARFROM"  QueryStringField="yearfrom" />
            <asp:QueryStringParameter DbType="String" DefaultValue="0"  Name="val" QueryStringField="val" />
            <asp:QueryStringParameter DefaultValue="1" Name="MONTH"  QueryStringField="mmonth" />
            <asp:QueryStringParameter DefaultValue="%" Name="SearchText"     QueryStringField="SearchText" />
			 <asp:QueryStringParameter  DefaultValue="ALL" Name="country"   QueryStringField="mcountry"  />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolderRightCol" Runat="Server">
</asp:Content>

