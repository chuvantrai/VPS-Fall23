import { useAxios } from '@/hooks';


const CITY_URI = 'api/City';
const COMMUNE_URI = 'api/Commune';
const DISTRICT_URI = 'api/District';
const useAddressServices = () => {
  const axios = useAxios();

  const getCities = (id = null) => {
    let uri = CITY_URI;
    if (id) {
      uri += '/' + id;
    }
    return axios.get(uri);
  };
  const getDistrictById = (id) => {
    let uri = DISTRICT_URI + '/' + id;
    return axios.get(uri);
  };
  const getCommuneById = (id) => {
    let uri = COMMUNE_URI + '/' + id;
    return axios.get(uri);
  };

  const getCommunes = (districtId = null) => {
    let commnueUri = COMMUNE_URI;
    if (districtId) commnueUri += `/GetByDistrict/${districtId}`;
    return axios.get(commnueUri);
  };
  const getDistricts = (cityId = null) => {
    let districtUri = DISTRICT_URI;
    if (cityId) districtUri += `/GetByCity/${cityId}`;
    return axios.get(districtUri);
  };

  const getAddressManager = (pageNumber = 1, pageSize = 10, cityFilter, districtFilter,
                             textSearch, addressType) => {
    let addressManager = COMMUNE_URI + `/GetAddressListParkingZone?`;
    addressManager += `pageNumber=${pageNumber}`;
    addressManager += `&pageSize=${pageSize}`;
    addressManager += `&typeAddress=${addressType}`;
    if (textSearch !== '') {
      addressManager += `&textAddress=${textSearch}`;
    }
    if (cityFilter !== undefined) {
      addressManager += `&cityFilter=${cityFilter}`;
    }
    if (districtFilter !== undefined) {
      addressManager += `&districtFilter=${districtFilter}`;
    }
    return axios.get(addressManager);
  };

  const updateIsBlockAddress = (isBlock, communeId, addressType) => {
    return axios.put(`${COMMUNE_URI}/UpdateIsBlockAddress`, {
      IsBlock: isBlock,
      CommuneId: communeId,
      TypeAddress: addressType,
    });
  };

  const createAddress = ({ type, name, city, district, code }) => {
    return axios.post(`${COMMUNE_URI}/CreateAddress`, {
      Type: type,
      Name: name,
      City: city,
      District: district,
      Code: code,
    });
  };

  return {
    getCities,
    getCommunes,
    getDistricts,
    getDistrictById,
    getCommuneById,
    getAddressManager,
    updateIsBlockAddress,
    createAddress,
  };

};
export default useAddressServices;