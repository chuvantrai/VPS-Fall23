import config from '@/config';
import { HeaderOnly, DefaultLayout } from '@/layouts';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';

import RegisterParkingZone from '@/pages/RegisterParkingZone';
import Profile from '@/pages/Homepage/components/Content/UserProfile';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ViewListParkingZone';

export const routes = [
  {
    path: config.routes.homepage,
    component: Homepage,
    layout: DefaultLayout,
    subRoutes: [
      { url: 'profile', component: Profile },
      { url: 'listParkingZone', component: ViewListParkingZone },
      { url: 'registerParkingZone', component: RegisterParkingZone },
    ],
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
