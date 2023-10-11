import config from '@/config';
import Login from '@/pages/Login/index.js';
import { HeaderOnly } from '@/layouts';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';
import RegisterParkingZone from '@/pages/RegisterParkingZone';
import ForgotPassword from '@/pages/ForgotPassword/index.js';

export const routes = [
  {
    path: config.routes.homepage,
    component: Homepage,
    layout: HeaderOnly,
  },
  {
    path: config.routes.register,
    component: Register,
    layout: null,
  },
  {
    path: config.routes.login,
    component: Login,
    layout: null,
  },
  {
    path: config.routes.verifyEmail,
    component: VerifyEmail,
    layout: null,
  },
  {
    path: config.routes.registerParkingZone,
    component: RegisterParkingZone,
    layout: HeaderOnly,
  },
  {
    path: config.routes.forgotPassword,
    component: ForgotPassword,
    layout: null,
  },
];