import Cookies from 'js-cookie';
import { keyNameCookies } from '@/helpers/index.js';

const getAccountDataByCookie = () => {
  const accountCode = Cookies.get(keyNameCookies.ACCOUNT_DATA);
  if (accountCode !== undefined && accountCode !== null && accountCode !== '') {
    return JSON.parse(atob(accountCode));
  }
  return null;
};

export default getAccountDataByCookie;