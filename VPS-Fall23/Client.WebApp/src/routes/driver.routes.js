import Header from '@/layouts/components/Header';
import Footer from '@/layouts/components/Footer';
import DriverHompage from '@/pages/Homepage/components/DriverHompage';
import DriverLayout from '../layouts/DriverLayout';
import guidGenerator from '../helpers/guidGenerator';
export const driverRoutesConfig = {
    header: Header,
    footer: Footer,
    layout: DriverLayout,
    routes: [
        {
            key: guidGenerator(),
            path: '',
            label: 'Trang chủ',
            component: DriverHompage
        }
    ]
}