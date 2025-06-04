import { Routes, Route } from "react-router-dom";
import MainLayout from "../layouts/MainLayout";
import Home from "../pages/Home";
import Register from "../pages/Register";
import { PATH_ADMIN, PATH_PUBLIC, PATH_USER } from "./path";
import Login from "../pages/Login";
import CardTemplate from "../pages/Card/CardTemplate";
import MainLayoutAdmin from "../layouts/MainLayoutAdmin";
import VerifyEmailPage from "../pages/VerifyEmailPage/VerifyEmailPage";
import ProfileUser from "../pages/ProfileUser";
import CardSingle from "../pages/CardSingle";
import CardDesign from "../pages/CardDesign";
import Guests from "../pages/Admin/Guests";
import InvitationWedding from "../pages/Admin/InvitationWedding";
import RSVPManagement from "../pages/Admin/RSVPManagement";
import TemplateSingle from "@/pages/Admin/TemplateSingle";
import CustomeCardPrenium from "@/pages/CustomCardPrenium";
import CustomCardFree from "@/pages/CustomCardFree";
import DashBoard from "@/pages/Admin/DashBoard";
import Users from "@/pages/Admin/Users";
import AuthGuard from "@/auth/AuthGruard";
import { RolesEnum } from "@/types/auth.type";
import InvivationTemplate from "@/pages/Admin/InvivationTemplate";

const GlobalRouter = () => {
  return (
    <Routes>
      <Route element={<MainLayout />}>
        <Route path={PATH_PUBLIC.home} element={<Home />} />
        <Route path={PATH_PUBLIC.card} element={<CardTemplate />} />
        <Route path={PATH_PUBLIC.cardSingle} element={<CardSingle />} />
        <Route path={PATH_PUBLIC.verifyEmail} element={<VerifyEmailPage />} />
        <Route path={PATH_PUBLIC.cardDesign} element={<CardDesign />} />
        <Route element={<AuthGuard roles={[RolesEnum.CUSTOMER]} />}>
          <Route path={PATH_PUBLIC.profileUser} element={<ProfileUser />} />
          <Route
            path={PATH_USER.customCardPre}
            element={<CustomeCardPrenium />}
          />
          <Route path={PATH_USER.customCardFree} element={<CustomCardFree />} />
        </Route>
      </Route>
      <Route path={PATH_PUBLIC.signUp} element={<Register />} />
      <Route path={PATH_PUBLIC.signIn} element={<Login />} />

      <Route element={<AuthGuard roles={[RolesEnum.ADMIN]} />}>
        <Route element={<MainLayoutAdmin></MainLayoutAdmin>}>
          <Route path={PATH_ADMIN.dashboard} element={<DashBoard />} />
          <Route path={PATH_ADMIN.guests} element={<Guests />} />
          <Route
            path={PATH_ADMIN.templatesWedding}
            element={<InvitationWedding />}
          />
          <Route path={PATH_ADMIN.rsvp} element={<RSVPManagement />} />
          <Route
            path={PATH_ADMIN.templateInvitation}
            element={<InvivationTemplate />}
          />
          <Route
            path={PATH_ADMIN.templateSingle}
            element={<TemplateSingle />}
          />
          <Route path={PATH_ADMIN.users} element={<Users />} />
        </Route>
      </Route>
    </Routes>

    // <Routes>
    //   <Route element={<Layout />}>

    //     {/* Public routes */}
    //     <Route index element={<HomePage></HomePage>} />
    //     <Route path={PATH_PUBLIC.signIn} element={<SignInPage></SignInPage>} />
    //     <Route path={PATH_PUBLIC.completeProfile} element={<CompleteProfile></CompleteProfile>} />
    //     <Route path={PATH_PUBLIC.forgotPassword} element={<ForgotPasswordPage></ForgotPasswordPage>} />
    //     <Route path={PATH_PUBLIC.verifyEmail} element={<VerifyEmailPage></VerifyEmailPage>} />
    //     <Route path={PATH_PUBLIC.signUpStudent} element={<SignUpStudentPage></SignUpStudentPage>} />
    //     <Route path={PATH_PUBLIC.signUpInstructor} element={<SignUpInstructor></SignUpInstructor>} />
    //     <Route path={PATH_PUBLIC.uploadDegree} element={<UploadDegreeInstructor></UploadDegreeInstructor>} />
    //     <Route path={PATH_PUBLIC.courses} element={<CoursesPage></CoursesPage>} />
    //     {/* Public routes */}

    //   </Route>

    //   <Route element={<AdminLayout />}>

    //     <Route element={<AuthGuard roles={[RolesEnum.ADMIN]} />}>
    //       {/* Admin routes */}
    //       <Route path={PATH_ADMIN.dashboard} element={<AdminDashboardPage></AdminDashboardPage>} />
    //       <Route path={PATH_ADMIN.categories} element={<CategoriesPage></CategoriesPage>} />
    //       <Route path={PATH_ADMIN.instructors} element={<InstructorsPage></InstructorsPage>} />
    //       <Route path={PATH_ADMIN.emails} element={<EmailTemplatesPage></EmailTemplatesPage>} />
    //       <Route path={PATH_ADMIN.emailsEdit} element={<EmailTemplateEditPage></EmailTemplateEditPage>} />
    //       <Route path={PATH_ADMIN.instructorInfo} element={<InstructorInfoPage></InstructorInfoPage>} />
    //       {/* Admin routes */}
    //     </Route>

    //   </Route>

    //   <Route element={<InstructorLayout />}>

    //     <Route element={<AuthGuard roles={[RolesEnum.INSTRUCTOR]} />}>
    //       {/* Instructor routes */}
    //       <Route path={PATH_INSTRUCTOR.dashboard} element={<InstructorDashBoardPage></InstructorDashBoardPage>} />
    //       <Route path={PATH_INSTRUCTOR.courses} element={<InstructorCoursesPage></InstructorCoursesPage>} />
    //       <Route path={PATH_INSTRUCTOR.courseVersions} element={<InstructorCourseVersionsPage></InstructorCourseVersionsPage>} />
    //       <Route path={PATH_INSTRUCTOR.courseVersionDetails} element={<CourseVersionDetailsPage></CourseVersionDetailsPage>} />
    //       <Route path={PATH_INSTRUCTOR.sectionVersionDetails} element={<SectionVersionDetailsPage></SectionVersionDetailsPage>} />
    //       {/* Instructor routes */}
    //     </Route>

    //   </Route>

    //   {/* Catch all (404) */}
    //   <Route path={PATH_PUBLIC.unauthorized} element={<UnauthorizedPage></UnauthorizedPage>} />
    //   <Route path={PATH_PUBLIC.notFound} element={<NotFoundPage />} />
    //   <Route path='*' element={<Navigate to={PATH_PUBLIC.notFound} replace />} />
    //   {/* Catch all (404) */}

    // </Routes>
  );
};

export default GlobalRouter;
