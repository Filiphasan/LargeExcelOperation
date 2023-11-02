using LargeExcelOperation.Core.Attributes;

namespace LargeExcelOperation.Core.Models.Report;

public class InvoiceReportSelectModel
{
    [ExcelTitle("Fatura No")]
    public string InvoiceNo { get; set; }

    [ExcelTitle("Fatura Tarihi")]
    public string InvoiceDate { get; set; }

    [ExcelTitle("E-Fatura Tarihi")]
    public string EInvoiceNumber { get; set; }

    [ExcelTitle("E-Fatura Tarihi")]
    public string EInvoiceDate { get; set; }

    [ExcelTitle("Müşteri Ad")]
    public string CustomerName { get; set; }

    [ExcelTitle("Müşteri Soyad")]
    public string CustomerSurname { get; set; }

    [ExcelTitle("Müşteri Gsm")]
    public string CustomerGsm { get; set; }

    [ExcelTitle("Müşteri Adres")]
    public string CustomerAddress { get; set; }

    [ExcelTitle("Sipariş No")]
    public string OrderNumber { get; set; }

    [ExcelTitle("Adet")]
    public string OrderQuantity { get; set; }

    [ExcelTitle("Ürün Ad")]
    public string ProductName { get; set; }

    [ExcelTitle("Ürün Açıklama")]
    public string ProductDescription { get; set; }

    [ExcelTitle("Ürün Fiyat")]
    public string ProductPrice { get; set; }

    [ExcelTitle("Ürün Barkod")]
    public string ProductBarcode { get; set; }

    [ExcelTitle("Ürün Kategori")]
    public string ProductCategory { get; set; }

    [ExcelTitle("Fiyat")]
    public string Price { get; set; }

    [ExcelTitle("KDV")]
    public string TaxPrice { get; set; }

    [ExcelTitle("Toplam Fiyat")]
    public string TotalPrice { get; set; }

    [ExcelTitle("Fatura Fiyat")]
    public string GrandPrice { get; set; }

    [ExcelTitle("Fatura KDV")]
    public string GrandTaxPrice { get; set; }

    [ExcelTitle("Fatura Toplam Fiyat")]
    public string GrandTotalPrice { get; set; }
}