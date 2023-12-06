import jwt_decode from 'jwt-decode';
import Cookies from 'js-cookie';
import { parse } from 'date-fns';
import keyFormatDate from '@/helpers/keyFormatDate.js';

const getAccountJwtModel = () => {
  const jwt = Cookies.get('ACCESS_TOKEN');
  if (jwt !== undefined && jwt !== '' && jwt !== null) {
    const decodedToken = jwt_decode(jwt);
    return {
      UserId: decodedToken.UserId,
      FirstName: decodedToken.FirstName,
      LastName: decodedToken.LastName,
      Email: decodedToken.Email,
      RoleId: decodedToken.RoleId,
      RoleName: decodedToken.RoleName,
      Avatar: decodedToken.Avatar,
      Expires: parse(decodedToken.Expires, keyFormatDate.DATE_KEY_JWT, new Date()),
      ModifiedAt: parse(decodedToken.ModifiedAt, keyFormatDate.DATE_KEY_JWT, new Date()),
    };
  } else {
    return null;
  }
};

export default getAccountJwtModel;
