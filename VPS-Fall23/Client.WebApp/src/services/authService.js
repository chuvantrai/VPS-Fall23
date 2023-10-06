import * as request from '@/helpers/httpRequest';

export const register = async (newAccount) => {
  try {
    const res = await request.post('/api/Auth/Register', {
      email: newAccount.email,
      password: newAccount.password,
      firstName: newAccount.firstName,
      lastName: newAccount.lastName,
      phoneNumber: newAccount.phoneNumber,
    });
  } catch (error) {
    console.log(error);
  }
};
