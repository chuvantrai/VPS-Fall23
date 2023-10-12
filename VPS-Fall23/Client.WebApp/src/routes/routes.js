import config from '@/config';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import VerifyEmail from '@/pages/VerifyEmail';
import { HeaderOnly, DefaultLayout } from '@/layouts';

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
];
