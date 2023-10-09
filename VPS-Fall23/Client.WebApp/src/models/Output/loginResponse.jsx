import PropTypes from 'prop-types';

const loginResponse = PropTypes.shape({
  accessToken: PropTypes.string.isRequired,
  userData: {
    avatar: PropTypes.string.isRequired = null,
    email: PropTypes.string.isRequired = null,
    expires: PropTypes.string.isRequired = null,
    firstName: PropTypes.string.isRequired = null,
    lastName: PropTypes.string.isRequired = null,
    modifiedAt: PropTypes.string.isRequired = null,
    roleId: PropTypes.number.isRequired = null,
    roleName: PropTypes.string.isRequired = null,
    userId: PropTypes.string.isRequired = null,
  }
});

PropTypes.checkPropTypes('loginResponse', loginResponse, 'props',
  'loginResponse');

export default loginResponse;