import config from '@/config';
import { HeaderOnly } from '@/layouts';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';

import RegisterParkingZone from '@/pages/RegisterParkingZone';

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
    path: config.routes.verifyEmail,
    component: VerifyEmail,
    layout: null,
  },
  {
    path: config.routes.registerParkingZone,
    component: RegisterParkingZone,
  },
];
