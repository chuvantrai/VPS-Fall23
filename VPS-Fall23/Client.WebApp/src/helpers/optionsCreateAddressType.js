import addressTypeEnum from "@/helpers/addressTypeEnum.js";

const optionsCreateAddressType = [
    { value: addressTypeEnum.CITY, label: 'Tỉnh/Thành phố' },
    { value: addressTypeEnum.DISTRICT, label: 'Quận/Huyện' },
    { value: addressTypeEnum.COMMUNE, label: 'Phường/Xã' },
];
export default optionsCreateAddressType;