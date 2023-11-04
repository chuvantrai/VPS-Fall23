import config from '@/config';
import { DefaultLayout } from '@/layouts';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';
import RegisterParkingZone from '@/pages/RegisterParkingZone';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ViewListParkingZone';
import ViewListOwner from '@/pages/Homepage/components/Content/ViewListParkingZoneOwner';
import ViewRequestedParkingZones from '@/pages/ViewRequestedParkingZones';
import Login from '@/pages/Login';
import ForgotPassword from '@/pages/ForgotPassword';
import ChangePassword from '@/pages/ChangePassword/index.js';
import AccountProfile from '@/pages/AccountProfile/index.js';
import userRoleEnum from '@/helpers/userRoleEnum.js';
import Test from '@/pages/Test/index.js';

export const routes = [
  {
    path: config.routes.homepage,
    component: Homepage,
    layout: DefaultLayout,
    subRoutes: [
      {
        url: 'profile',
        component: AccountProfile,
      },
      {
        url: 'listParkingZone',
        component: ViewListParkingZone,
      },
      {
        url: 'listOwner',
        component: ViewListOwner,
      },
      {
        url: 'registerParkingZone',
        component: RegisterParkingZone,
        userRole: [userRoleEnum.OWNER],
      },
      {
        url: 'viewRequestedParkingZones',
        component: ViewRequestedParkingZones,
      },
      {
        url: 'test',
        component: Test,
      },
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
    path: config.routes.login,
    component: Login,
    layout: null,
  },
  {
    path: config.routes.forgotPassword,
    component: ForgotPassword,
    layout: null,
  },
  {
    path: config.routes.changePassword,
    component: ChangePassword,
    layout: null,
  },
];
