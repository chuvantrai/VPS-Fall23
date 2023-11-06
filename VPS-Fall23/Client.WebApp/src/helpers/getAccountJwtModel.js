import jwt_decode from 'jwt-decode';
import Cookies from 'js-cookie';

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
      Expires: new Date(decodedToken.Expires),
      ModifiedAt: new Date(decodedToken.ModifiedAt),
    };
  } else {
    return null;
  }
};

export default getAccountJwtModel;
