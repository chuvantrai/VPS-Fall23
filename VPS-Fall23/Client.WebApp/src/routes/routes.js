import config from '@/config';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';
import Login from '@/pages/Login';
import ForgotPassword from '@/pages/ForgotPassword';
import { DefaultLayout } from '@/layouts';

export const routes = [
  {
    path: config.routes.homepage,
    component: Homepage,
    layout: DefaultLayout,
  },
  {
    path: config.routes.register,
    component: Register,
    layout: null,
  },
  {
    path: config.routes.verifyEmail,
    component: VerifyEmail,
    layout: null,
  },
  {
    path: config.routes.login,
    component: Login,
    layout: null,
  },
  {
    path: config.routes.forgotPassword,
    component: ForgotPassword,
    layout: null,
  }
];
