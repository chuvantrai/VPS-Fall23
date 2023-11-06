import Header from '@/layouts/components/Header';
import AccountProfile from '@/pages/AccountProfile';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ViewListParkingZone';
import ViewListParkingZoneOwner from '@/pages/Homepage/components/Content/ViewListParkingZoneOwner';
import ViewRequestedParkingZone from '@/pages/ViewRequestedParkingZones';
import ManagerLayout from '../layouts/ManagerLayout';
import guidGenerator from '../helpers/guidGenerator';
export const adminRoutesConfig = {
  header: Header,
  footer: null,
  layout: ManagerLayout,
  routes: [
    {
      key: guidGenerator(),
      path: 'user',
      label: 'User',
      children: [
        {
          key: guidGenerator(),
          path: 'profile',
          label: 'Profile',
          component: AccountProfile,
          description: 'Mọi thông tin về tài khoản được hiển thị dưới đây',
        },
      ],
    },
    {
      key: guidGenerator(),
      path: 'parking-zone',
      label: 'Bãi đỗ xe',
      children: [
        {
          key: guidGenerator(),
          path: '',
          label: 'Danh sách bãi đỗ xe',
          description: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
          component: ViewListParkingZone,
        },
        {
          key: guidGenerator(),
          path: 'parking-zone-owner',
          label: 'Danh sách chủ bãi gửi xe',
          description: 'Toàn bộ danh sách chủ bãi gửi xe hiển thị dưới đây',
          component: ViewListParkingZoneOwner,
        },
        {
          key: guidGenerator(),
          path: 'requested-list',
          label: 'Danh sách yêu cầu',
          description: 'Toàn bộ danh sách yêu cầu bãi gửi xe hiển thị dưới đây',
          component: ViewRequestedParkingZone,
        },
      ],
    },
  ],
};
