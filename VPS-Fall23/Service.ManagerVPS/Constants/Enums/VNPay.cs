namespace Service.ManagerVPS.Constants.Enums.VNPay
{
    public enum BankCode
    {
        VNPAYQR,
        VNBANK,
        INTCARD
    }
    public static class Constant
    {
        public static class Key
        {
            public const string Command = "vnp_Command";
            public const string BankCode = "vnp_BankCode";
            public const string IpAddress = "vnp_IpAddr";
            public const string CreateDate = "vnp_CreateDate";
            public const string ExpireDate = "vnp_ExpireDate";
            public const string TxnRef = "vnp_TxnRef"; //Mã giao dịch
            public const string Amount = "vnp_Amount";
            public const string OrderInfo = "vnp_OrderInfo";
            public const string SecureHash = "vnp_SecureHash";
        }
        public static class Value
        {
            public const string Command = "pay";
        }
    }

}
