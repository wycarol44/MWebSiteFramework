<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Main.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" ClientIDMode="AutoID" %>
<%@ Register Assembly="System.Web.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style type ="text/css" >
        .FeaturedPicture{
            align-content:center; 
            align-items:center;
            align-self:center;   
        }
        
        td {
             padding: 10px;
        }

        /*.SDes{
            align-content:center; 
            float:right; 
        }*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="Server">
      
    <h1>Featured Products</h1>

    <br />
     <telerik:RadRotator runat="server" ID="RadRotator" FrameDuration="1000" ScrollDuration="5000" RotatorType="AutomaticAdvance" Width="600px"  Height="180px" ItemWidth="180px" ItemHeight="180px" BackColor ="Black" CssClass ="FeaturedPicture"  >
         <ItemTemplate>
             <asp:Image runat ="server"  ID="Image" ImageUrl =<%# String.Format("~/Documents/Pictures/{0}", Eval("ThumbnailPath"))%> ></asp:Image>

<%--             <br />
             <asp:Label ID="lblSDes" runat="server" Text='<%# Eval("ShortDescription") %>' ForeColor ="White" CssClass ="SDes"></asp:Label>--%>

         </ItemTemplate>
     </telerik:RadRotator>


<%--    Product List  --%>
    <br />
    <br />
        <telerik:RadListView ID="RadListView1" runat="server" ItemPlaceholderID="ProductsContainer">
        <LayoutTemplate>
            <fieldset style="width: 900px">
                <legend style="font:x-large ">Products</legend>
                <asp:PlaceHolder ID="ProductsContainer" runat="server"></asp:PlaceHolder>
            </fieldset>
        </LayoutTemplate>
        <ItemTemplate>
            <fieldset style="float: left; width: 420px;">
                <legend style="font:small-caption" >
                    Category:
                        <%#Eval("CategoryName")%>,
                    SubCategory:
                        <%#Eval("SubCategoryName")%>
                </legend>
                <table>
                    <tr>
                        <td>
                            <table>
                                <tr>
                                    <td style="width: 25%">Name:
                                    </td>
                                    <td style="width: 50%">
                                        <%#Eval("ProductName")%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Price:
                                    </td>
                                    <td>
                                       <%#FormatCurrency(Eval("Price"),2)%>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Buy:
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lbtnAddToCart" runat="server" CommandName = "AddToCart">Add To Cart</asp:LinkButton>
                                    </td>
                                </tr>
                               
                            </table>
                        </td>
                        <td style="width: 25%; padding-left: 10px;">
                               <asp:Image runat ="server" ID="Image" ImageUrl =<%# String.Format("~/Documents/Pictures/{0}", Eval("ThumbnailPath"))%>  Width="150px" Height ="150px" BorderWidth="2px" BorderStyle="Groove"   ></asp:Image>
                        </td>
                    </tr>
                </table>

                <asp:Label ID="lblProductID" runat="server" Text= <%#Eval("ProductID")%> Visible ="false" ></asp:Label>

            </fieldset>
        </ItemTemplate>
</telerik:RadListView>


 

</asp:Content>

